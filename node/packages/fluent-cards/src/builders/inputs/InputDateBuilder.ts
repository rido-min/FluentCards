import { Spacing } from '../../enums.js';
import type { InputDate } from '../../models.js';

/** Fluent builder for {@link InputDate} elements. */
export class InputDateBuilder {
  private readonly input: InputDate = { type: 'Input.Date', id: '' };

  withId(id: string): this { this.input.id = id; return this; }
  withLabel(label: string): this { this.input.label = label; return this; }
  withPlaceholder(placeholder: string): this { this.input.placeholder = placeholder; return this; }
  withValue(value: string): this { this.input.value = value; return this; }
  withMin(min: string): this { this.input.min = min; return this; }
  withMax(max: string): this { this.input.max = max; return this; }
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  build(): InputDate { return this.input; }
}
