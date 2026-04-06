import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    InputTextBuilder,
    InputToggleBuilder,
    InputNumberBuilder,
    InputDateBuilder,
    validate,
    validate_and_throw,
    AdaptiveCardValidationError,
    ValidationSeverity,
    ValidationIssue,
)


class TestValidate:
    def test_returns_no_issues_for_valid_card(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_text_block(lambda b: b.with_text('Hello World'))
                .build())
        issues = validate(card)
        assert len(issues) == 0

    def test_reports_missing_version_error_when_version_is_empty(self):
        card = {'type': 'AdaptiveCard', 'version': ''}
        issues = validate(card)
        err = next((i for i in issues if i.code == 'MISSING_VERSION'), None)
        assert err is not None
        assert err.severity == ValidationSeverity.Error
        assert err.path == 'version'

    def test_reports_missing_schema_warning_when_schema_is_absent(self):
        card = {'type': 'AdaptiveCard', 'version': '1.5', '$schema': None,
                'body': [{'type': 'TextBlock', 'text': 'Hi'}]}
        issues = validate(card)
        warn = next((i for i in issues if i.code == 'MISSING_SCHEMA'), None)
        assert warn is not None
        assert warn.severity == ValidationSeverity.Warning
        assert warn.path == '$schema'

    def test_reports_empty_card_warning_when_body_and_actions_are_absent(self):
        card = {'type': 'AdaptiveCard', 'version': '1.5'}
        issues = validate(card)
        assert any(i.code == 'EMPTY_CARD' and i.severity == ValidationSeverity.Warning for i in issues)

    def test_reports_missing_text_error_for_blank_text_block(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_text_block(lambda b: b.with_text(''))
                .build())
        issues = validate(card)
        err = next((i for i in issues if i.code == 'MISSING_TEXT'), None)
        assert err is not None
        assert err.severity == ValidationSeverity.Error
        assert err.path == 'body[0].text'

    def test_reports_missing_image_url_error_for_image_without_url(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_image(lambda b: b.with_url(''))
                .build())
        issues = validate(card)
        err = next((i for i in issues if i.code == 'MISSING_IMAGE_URL'), None)
        assert err is not None
        assert err.severity == ValidationSeverity.Error
        assert err.path == 'body[0].url'

    def test_reports_invalid_image_url_warning_for_relative_url(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_image(lambda b: b.with_url('not-a-url'))
                .build())
        issues = validate(card)
        warn = next((i for i in issues if i.code == 'INVALID_IMAGE_URL'), None)
        assert warn is not None
        assert warn.severity == ValidationSeverity.Warning
        assert warn.path == 'body[0].url'

    def test_reports_missing_input_id_error_for_input_without_id(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_input_text(lambda b: b.with_placeholder('Enter text'))
                .build())
        issues = validate(card)
        err = next((i for i in issues if i.code == 'MISSING_INPUT_ID'), None)
        assert err is not None
        assert err.severity == ValidationSeverity.Error
        assert err.path == 'body[0].id'

    def test_reports_too_many_actions_warning_when_more_than_5_actions(self):
        builder = (AdaptiveCardBuilder.create()
                   .with_version('1.5')
                   .add_text_block(lambda b: b.with_text('Test')))
        for i in range(6):
            builder.add_action(lambda a, i=i: a.open_url(f'https://example.com/{i}').with_title(f'Action {i}'))
        issues = validate(builder.build())
        warn = next((i for i in issues if i.code == 'TOO_MANY_ACTIONS'), None)
        assert warn is not None
        assert warn.severity == ValidationSeverity.Warning
        assert warn.path == 'actions'

    def test_reports_missing_action_url_error_for_open_url_without_url(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_text_block(lambda b: b.with_text('Test'))
                .build())
        card['actions'] = [{'type': 'Action.OpenUrl', 'url': ''}]
        issues = validate(card)
        err = next((i for i in issues if i.code == 'MISSING_ACTION_URL'), None)
        assert err is not None
        assert err.path == 'actions[0].url'

    def test_reports_invalid_action_url_warning_for_relative_open_url(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_text_block(lambda b: b.with_text('Test'))
                .add_action(lambda a: a.open_url('not-a-url').with_title('Test'))
                .build())
        issues = validate(card)
        warn = next((i for i in issues if i.code == 'INVALID_ACTION_URL'), None)
        assert warn is not None
        assert warn.path == 'actions[0].url'

    def test_reports_unknown_version_warning_for_unrecognised_version(self):
        card = {
            'type': 'AdaptiveCard',
            'version': '9.9',
            'body': [{'type': 'TextBlock', 'text': 'Test'}],
        }
        issues = validate(card)
        warn = next((i for i in issues if i.code == 'UNKNOWN_VERSION'), None)
        assert warn is not None
        assert warn.severity == ValidationSeverity.Warning

    def test_validates_nested_container_items(self):
        input_no_id = InputTextBuilder().with_placeholder('Test').build()
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_container(lambda c: c
                               .add_text_block(lambda b: b.with_text(''))
                               .add_element(input_no_id))
                .build())
        issues = validate(card)
        assert any(i.code == 'MISSING_TEXT' and i.path == 'body[0].items[0].text' for i in issues)
        assert any(i.code == 'MISSING_INPUT_ID' and i.path == 'body[0].items[1].id' for i in issues)

    def test_validates_nested_columnset_items(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_column_set(lambda cs: cs
                                .add_column(lambda col: col.add_image(lambda img: img.with_url(''))))
                .build())
        issues = validate(card)
        err = next((i for i in issues if i.code == 'MISSING_IMAGE_URL'), None)
        assert err is not None
        assert err.path == 'body[0].columns[0].items[0].url'


class TestValidateAndThrow:
    def test_throws_adaptive_card_validation_error_when_errors_exist(self):
        card = {'type': 'AdaptiveCard', 'version': ''}
        with pytest.raises(AdaptiveCardValidationError):
            validate_and_throw(card)

    def test_does_not_throw_for_warnings_only_cards(self):
        card = {
            'type': 'AdaptiveCard',
            'version': '1.5',
            '$schema': None,
            'body': [{'type': 'TextBlock', 'text': 'Test'}],
        }
        validate_and_throw(card)  # Should not raise

    def test_does_not_throw_for_fully_valid_card(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_text_block(lambda b: b.with_text('Hello World'))
                .build())
        validate_and_throw(card)  # Should not raise


class TestAdaptiveCardValidationError:
    def test_formats_single_error_correctly(self):
        err = AdaptiveCardValidationError([
            ValidationIssue(severity=ValidationSeverity.Error, code='TEST',
                            path='test.path', message='Test error message'),
        ])
        assert err.message == 'Adaptive Card validation failed: Test error message'

    def test_formats_multiple_errors_correctly(self):
        err = AdaptiveCardValidationError([
            ValidationIssue(severity=ValidationSeverity.Error, code='T1', path='path1', message='Error 1'),
            ValidationIssue(severity=ValidationSeverity.Error, code='T2', path='path2', message='Error 2'),
        ])
        assert 'Adaptive Card validation failed with 2 errors' in err.message
        assert '[path1] Error 1' in err.message
        assert '[path2] Error 2' in err.message

    def test_exposes_errors_array(self):
        errors = [
            ValidationIssue(severity=ValidationSeverity.Error, code='T1', path='p', message='M'),
        ]
        ex = AdaptiveCardValidationError(errors)
        assert ex.errors == errors


class TestValidationNewRules:
    def test_reports_missing_toggle_title_error(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_input_toggle(lambda b: b.with_id('t1'))
                .build())
        issues = validate(card)
        assert any(i.code == 'MISSING_TOGGLE_TITLE' and i.severity == ValidationSeverity.Error
                   for i in issues)

    def test_reports_min_greater_than_max_for_input_number(self):
        input_ = InputNumberBuilder().with_id('n1').with_min(10).with_max(5).build()
        card = AdaptiveCardBuilder.create().with_version('1.5').add_element(input_).build()
        issues = validate(card)
        assert any(i.code == 'MIN_GREATER_THAN_MAX' and i.severity == ValidationSeverity.Error
                   for i in issues)

    def test_reports_min_greater_than_max_for_input_date(self):
        input_ = InputDateBuilder().with_id('d1').with_min('2024-12-31').with_max('2024-01-01').build()
        card = AdaptiveCardBuilder.create().with_version('1.5').add_element(input_).build()
        issues = validate(card)
        assert any(i.code == 'MIN_GREATER_THAN_MAX' for i in issues)

    def test_reports_missing_facts_error_for_empty_fact_set(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_fact_set(lambda fs: fs)
                .build())
        issues = validate(card)
        assert any(i.code == 'MISSING_FACTS' and i.severity == ValidationSeverity.Error for i in issues)

    def test_reports_missing_inlines_error_for_empty_rich_text_block(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_rich_text_block(lambda b: b)
                .build())
        issues = validate(card)
        assert any(i.code == 'MISSING_INLINES' and i.severity == ValidationSeverity.Error for i in issues)

    def test_reports_invalid_select_action_for_show_card_as_select_action(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_text_block(lambda b: b.with_text('Hello'))
                .with_select_action({'type': 'Action.ShowCard', 'title': 'Show'})
                .build())
        issues = validate(card)
        assert any(i.code == 'INVALID_SELECT_ACTION' and i.severity == ValidationSeverity.Error
                   for i in issues)

    def test_reports_duplicate_id_warning_when_two_elements_share_an_id(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.5')
                .add_text_block(lambda b: b.with_id('same').with_text('First'))
                .add_text_block(lambda b: b.with_id('same').with_text('Second'))
                .build())
        issues = validate(card)
        assert any(i.code == 'DUPLICATE_ID' and i.severity == ValidationSeverity.Warning for i in issues)

    def test_reports_version_mismatch_warning_for_table_in_v1_0_card(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.0')
                .add_table(lambda t: t)
                .build())
        issues = validate(card)
        assert any(i.code == 'VERSION_MISMATCH' and i.path.startswith('body[0]') for i in issues)

    def test_reports_version_mismatch_for_refresh_used_with_v1_0(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.0')
                .add_text_block(lambda b: b.with_text('Hello'))
                .with_refresh(lambda r: r.with_action(lambda a: a.execute('Refresh')))
                .build())
        issues = validate(card)
        assert any(i.code == 'VERSION_MISMATCH' and i.path == 'refresh' for i in issues)
