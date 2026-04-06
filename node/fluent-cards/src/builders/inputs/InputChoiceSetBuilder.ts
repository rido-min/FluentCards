import type { InputChoiceSet, Choice } from '../../models.js';
import { ChoiceInputStyle, Spacing } from '../../enums.js';

/** Fluent builder for {@link InputChoiceSet} elements. */
export class InputChoiceSetBuilder {
  private readonly input: InputChoiceSet = { type: 'Input.ChoiceSet', id: '', choices: [] };

  withId(id: string): this { this.input.id = id; return this; }
  withLabel(label: string): this { this.input.label = label; return this; }
  withPlaceholder(placeholder: string): this { this.input.placeholder = placeholder; return this; }
  withValue(value: string): this { this.input.value = value; return this; }
  withStyle(style: ChoiceInputStyle): this { this.input.style = style; return this; }
  isMultiSelect(isMultiSelect = true): this { this.input.isMultiSelect = isMultiSelect; return this; }
  withWrap(wrap = true): this { this.input.wrap = wrap; return this; }
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  addChoice(title: string, value: string): this;
  addChoice(choice: Choice): this;
  addChoice(titleOrChoice: string | Choice, value?: string): this {
    if (typeof titleOrChoice === 'string') {
      this.input.choices!.push({ title: titleOrChoice, value: value! });
    } else {
      this.input.choices!.push(titleOrChoice);
    }
    return this;
  }

  build(): InputChoiceSet { return this.input; }
}
