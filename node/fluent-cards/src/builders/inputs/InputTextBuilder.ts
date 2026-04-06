import type { InputText, AdaptiveAction } from '../../models.js';
import { TextInputStyle, Spacing } from '../../enums.js';
import { ActionBuilder } from '../ActionBuilder.js';

/** Fluent builder for {@link InputText} elements. */
export class InputTextBuilder {
  private readonly input: InputText = { type: 'Input.Text', id: '' };

  withId(id: string): this { this.input.id = id; return this; }
  withLabel(label: string): this { this.input.label = label; return this; }
  withPlaceholder(placeholder: string): this { this.input.placeholder = placeholder; return this; }
  withValue(value: string): this { this.input.value = value; return this; }
  withMaxLength(maxLength: number): this { this.input.maxLength = maxLength; return this; }
  withIsMultiline(isMultiline = true): this { this.input.isMultiline = isMultiline; return this; }
  withStyle(style: TextInputStyle): this { this.input.style = style; return this; }
  withRegex(regex: string): this { this.input.regex = regex; return this; }
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  withInlineAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.input.inlineAction = b.build();
    return this;
  }

  build(): InputText { return this.input; }
}
