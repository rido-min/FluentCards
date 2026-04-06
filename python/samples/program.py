from fluent_cards import (
    AdaptiveCardBuilder,
    to_json,
    from_json,
    validate,
    TextSize,
    TextWeight,
    TextColor,
)

from basic_card_sample import create_welcome_card, create_notification_card, create_image_card
from form_card_sample import create_contact_form, create_survey_form, create_registration_form
from layout_card_sample import (
    create_two_column_card,
    create_styled_container_card,
    create_fact_set_card,
    create_nested_container_card,
)
from rich_content_sample import (
    create_rich_text_card,
    create_image_set_card,
    create_table_card,
    create_media_card,
    create_comprehensive_card,
)

print('=== FluentCards Demo ===\n')

# Create a card using the fluent builder pattern
card = (
    AdaptiveCardBuilder.create()
    .with_version('1.5')
    .add_text_block(
        lambda tb: tb.with_text('Hello, FluentCards!')
        .with_size(TextSize.Large)
        .with_weight(TextWeight.Bolder)
        .with_wrap(True)
    )
    .add_text_block(
        lambda tb: tb.with_text('This card was built with a fluent interface.').with_color(
            TextColor.Accent
        )
    )
    .add_action(
        lambda a: a.open_url('https://adaptivecards.io').with_title('Learn More')
    )
    .build()
)

# Serialize to JSON
json_str = to_json(card)
print(json_str)

# Demonstrate roundtrip serialization
print('\n=== Roundtrip Test ===')
deserialized = from_json(json_str)
if deserialized:
    print('\u2713 Successfully deserialized card')
    print(f"  Version: {deserialized['version']}")
    print(f"  Body elements: {len(deserialized.get('body', []))}")
    print(f"  Actions: {len(deserialized.get('actions', []))}")

# Demonstrate validation
print('\n=== Validation Test ===')
issues = validate(card)
if len(issues) == 0:
    print('\u2713 Card is valid!')
else:
    print(f'\u26a0 Found {len(issues)} validation issue(s):')
    for issue in issues:
        print(f'  [{issue.severity}] {issue.path}: {issue.message}')

# Demonstrate validation with invalid card
print('\n=== Validation with Invalid Card ===')
invalid_card = {'type': 'AdaptiveCard', 'version': ''}
invalid_issues = validate(invalid_card)
print(f'Found {len(invalid_issues)} validation issue(s):')
for issue in invalid_issues:
    print(f"  [{issue.severity}] {issue.code} at '{issue.path}': {issue.message}")


# Run all samples and print their JSON
def print_sample(name: str, card: dict):
    print(f'\n=== {name} ===')
    print(to_json(card))


print_sample('Welcome Card', create_welcome_card())
print_sample('Notification Card', create_notification_card())
print_sample('Image Card', create_image_card())
print_sample('Contact Form', create_contact_form())
print_sample('Survey Form', create_survey_form())
print_sample('Registration Form', create_registration_form())
print_sample('Two Column Card', create_two_column_card())
print_sample('Styled Container Card', create_styled_container_card())
print_sample('Fact Set Card', create_fact_set_card())
print_sample('Nested Container Card', create_nested_container_card())
print_sample('Rich Text Card', create_rich_text_card())
print_sample('Image Set Card', create_image_set_card())
print_sample('Table Card', create_table_card())
print_sample('Media Card', create_media_card())
print_sample('Comprehensive Card', create_comprehensive_card())
