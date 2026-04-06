import {
  AdaptiveCardBuilder,
  validate,
  validateAndThrow,
  AdaptiveCardValidationError,
  ValidationSeverity,
  TextSize,
  TextWeight,
} from 'fluent-cards';
import type { AdaptiveCard, ValidationIssue } from 'fluent-cards';

/** Runs all validation demonstrations. */
export function runValidationSamples() {
  demonstrateValidCard();
  demonstrateStructuralErrors();
  demonstrateInvalidInputRange();
  demonstrateVersionMismatch();
  demonstrateValidateAndThrow();
}

/** Validates a well-formed card — expects zero issues. */
function demonstrateValidCard() {
  console.log('\n=== Validation: Valid Card ===');

  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) => tb.withText('All good!').withSize(TextSize.Large).withWrap(true))
    .addAction((a) => a.openUrl('https://adaptivecards.io').withTitle('Learn More'))
    .build();

  printIssues(validate(card));
}

/** Validates a card with missing required fields — expects multiple errors. */
function demonstrateStructuralErrors() {
  console.log('\n=== Validation: Structural Errors ===');

  // Intentional problems: no version, TextBlock with no text, Image with no URL
  const card: AdaptiveCard = {
    type: 'AdaptiveCard',
    version: '',
    body: [
      { type: 'TextBlock', text: '' },
      { type: 'Image', url: '' },
    ],
  };

  printIssues(validate(card));
}

/** Validates an Input.Number with min greater than max — expects a range error. */
function demonstrateInvalidInputRange() {
  console.log('\n=== Validation: Invalid Input Range ===');

  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addInputNumber((i) => i.withId('qty').withLabel('Quantity').withMin(100).withMax(10))
    .build();

  printIssues(validate(card));
}

/** Validates a card that uses elements requiring a newer schema version — expects version mismatch warnings. */
function demonstrateVersionMismatch() {
  console.log('\n=== Validation: Version Mismatch ===');

  // Table requires v1.5; declaring v1.0 should trigger a VERSION_MISMATCH warning
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.0')
    .addTextBlock((tb) => tb.withText('Sales Report').withWeight(TextWeight.Bolder))
    .addTable((table) => {
      table
        .addColumn({ width: '1' })
        .addColumn({ width: '1' })
        .addRow({
          type: 'TableRow',
          cells: [
            { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Product' }] },
            { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Sales' }] },
          ],
        });
    })
    .build();

  printIssues(validate(card));
}

/** Demonstrates validateAndThrow — catches the error thrown on validation errors. */
function demonstrateValidateAndThrow() {
  console.log('\n=== Validation: validateAndThrow ===');

  const card: AdaptiveCard = { type: 'AdaptiveCard', version: '' };

  try {
    validateAndThrow(card);
    console.log('No errors found.');
  } catch (err) {
    if (err instanceof AdaptiveCardValidationError) {
      console.log('Caught AdaptiveCardValidationError:');
      for (const error of err.errors) {
        console.log(`  [${error.code}] ${error.message}`);
      }
    }
  }
}

function printIssues(issues: ValidationIssue[]) {
  if (issues.length === 0) {
    console.log('✓ Card is valid — no issues found.');
    return;
  }
  console.log(`Found ${issues.length} issue(s):`);
  for (const issue of issues) {
    const icon = issue.severity === ValidationSeverity.Error ? '✗' : '⚠';
    console.log(`  ${icon} [${issue.severity}] ${issue.code} at '${issue.path}': ${issue.message}`);
  }
}
