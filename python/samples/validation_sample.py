from fluent_cards import (
    AdaptiveCardBuilder,
    validate,
    validate_and_throw,
    AdaptiveCardValidationError,
    ValidationSeverity,
    TextSize,
    TextWeight,
)


def run_validation_samples() -> None:
    """Runs all validation demonstrations."""
    _demonstrate_valid_card()
    _demonstrate_structural_errors()
    _demonstrate_invalid_input_range()
    _demonstrate_version_mismatch()
    _demonstrate_validate_and_throw()


def _demonstrate_valid_card() -> None:
    """Validates a well-formed card — expects zero issues."""
    print('\n=== Validation: Valid Card ===')

    card = (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(lambda tb: tb.with_text('All good!').with_size(TextSize.Large).with_wrap(True))
        .add_action(lambda a: a.open_url('https://adaptivecards.io').with_title('Learn More'))
        .build()
    )

    _print_issues(validate(card))


def _demonstrate_structural_errors() -> None:
    """Validates a card with missing required fields — expects multiple errors."""
    print('\n=== Validation: Structural Errors ===')

    # Intentional problems: no version, TextBlock with no text, Image with no URL
    card = {
        'type': 'AdaptiveCard',
        'version': '',
        'body': [
            {'type': 'TextBlock', 'text': ''},
            {'type': 'Image', 'url': ''},
        ],
    }

    _print_issues(validate(card))


def _demonstrate_invalid_input_range() -> None:
    """Validates an Input.Number with min greater than max — expects a range error."""
    print('\n=== Validation: Invalid Input Range ===')

    card = (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_input_number(
            lambda i: i.with_id('qty').with_label('Quantity').with_min(100).with_max(10)
        )
        .build()
    )

    _print_issues(validate(card))


def _demonstrate_version_mismatch() -> None:
    """Validates a card that uses elements requiring a newer schema version — expects version mismatch warnings."""
    print('\n=== Validation: Version Mismatch ===')

    # Table requires v1.5; declaring v1.0 should trigger a VERSION_MISMATCH warning
    card = (
        AdaptiveCardBuilder.create()
        .with_version('1.0')
        .add_text_block(lambda tb: tb.with_text('Sales Report').with_weight(TextWeight.Bolder))
        .add_table(
            lambda table: table
            .add_column({'width': '1'})
            .add_column({'width': '1'})
            .add_row({
                'type': 'TableRow',
                'cells': [
                    {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': 'Product'}]},
                    {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': 'Sales'}]},
                ],
            })
        )
        .build()
    )

    _print_issues(validate(card))


def _demonstrate_validate_and_throw() -> None:
    """Demonstrates validate_and_throw — catches the exception raised on validation errors."""
    print('\n=== Validation: validate_and_throw ===')

    card = {'type': 'AdaptiveCard', 'version': ''}

    try:
        validate_and_throw(card)
        print('No errors found.')
    except AdaptiveCardValidationError as ex:
        print('Caught AdaptiveCardValidationError:')
        for error in ex.errors:
            print(f'  [{error.code}] {error.message}')


def _print_issues(issues: list) -> None:
    if not issues:
        print('✓ Card is valid — no issues found.')
        return
    print(f'Found {len(issues)} issue(s):')
    for issue in issues:
        icon = '✗' if issue.severity == ValidationSeverity.Error else '⚠'
        print(f"  {icon} [{issue.severity.value}] {issue.code} at '{issue.path}': {issue.message}")
