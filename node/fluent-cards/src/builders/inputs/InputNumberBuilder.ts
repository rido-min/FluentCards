import { Spacing } from '../../enums.js';
import type { InputNumber } from '../../models.js';

/** Fluent builder for {@link InputNumber} elements. */
export class InputNumberBuilder {
  private readonly input: InputNumber = { type: 'Input.Number', id: '' };

  withId(id: string): this { this.input.id = id; return this; }
  withLabel(label: string): this { this.input.label = label; return this; }
  withPlaceholder(placeholder: string): this { this.input.placeholder = placeholder; return this; }
  withValue(value: number): this { this.input.value = value; return this; }
  withMin(min: number): this { this.input.min = min; return this; }
  withMax(max: number): this { this.input.max = max; return this; }
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  build(): InputNumber { return this.input; }
}
