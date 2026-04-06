import type { InputChoiceSet, Choice, DataQuery } from '../../models.js';
import { ChoiceInputStyle, Spacing } from '../../enums.js';

/** Fluent builder for {@link InputChoiceSet} elements. */
export class InputChoiceSetBuilder {
  private readonly input: InputChoiceSet = { type: 'Input.ChoiceSet', id: '', choices: [] };

  /** Sets the unique identifier (required for form submission). @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.input.id = id; return this; }
  /** Sets the descriptive label displayed above the input. @param label The label text. @returns The builder instance for method chaining. */
  withLabel(label: string): this { this.input.label = label; return this; }
  /** Sets the placeholder text shown when no choice is selected. @param placeholder The placeholder text. @returns The builder instance for method chaining. */
  withPlaceholder(placeholder: string): this { this.input.placeholder = placeholder; return this; }
  /** Sets the initial selected value. @param value The initial value. @returns The builder instance for method chaining. */
  withValue(value: string): this { this.input.value = value; return this; }
  /** Sets the display style (compact dropdown or expanded list). @param style The choice input style. @returns The builder instance for method chaining. */
  withStyle(style: ChoiceInputStyle): this { this.input.style = style; return this; }
  /** Sets whether multiple choices can be selected. @param isMultiSelect True to allow multi-select. @returns The builder instance for method chaining. */
  isMultiSelect(isMultiSelect = true): this { this.input.isMultiSelect = isMultiSelect; return this; }
  /** Sets whether choice labels wrap. @param wrap True to allow wrapping. @returns The builder instance for method chaining. */
  withWrap(wrap = true): this { this.input.wrap = wrap; return this; }
  /** Sets whether a selection is required. @param isRequired True if required. @returns The builder instance for method chaining. */
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  /** Sets the error message shown when validation fails. @param errorMessage The error message. @returns The builder instance for method chaining. */
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  /** Adds a choice with the given title and value. @param title The choice label displayed to the user. @param value The choice value submitted on selection. @returns The builder instance for method chaining. */
  addChoice(title: string, value: string): this;
  /** Adds a pre-built Choice object. @param choice A pre-built Choice. @returns The builder instance for method chaining. */
  addChoice(choice: Choice): this;
  addChoice(titleOrChoice: string | Choice, value?: string): this {
    if (typeof titleOrChoice === 'string') {
      this.input.choices!.push({ title: titleOrChoice, value: value! });
    } else {
      this.input.choices!.push(titleOrChoice);
    }
    return this;
  }

  /** Sets a dynamic data query for fetching choices from a data source (Adaptive Cards 1.6+). @param dataset The dataset identifier, e.g. `"graph.microsoft.com/users"`. @returns The builder instance for method chaining. */
  withChoicesData(dataset: string): this { this.input['choices.data'] = { type: 'Data.Query', dataset } as DataQuery; return this; }

  /** Builds and returns the configured InputChoiceSet element. @returns The configured InputChoiceSet instance. */
  build(): InputChoiceSet { return this.input; }
}
