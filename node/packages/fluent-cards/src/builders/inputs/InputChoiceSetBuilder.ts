import type { InputChoiceSet, Choice, DataQuery, AdaptiveElement } from '../../models.js';
import { ChoiceInputStyle, InputLabelPosition, InputStyle, Spacing } from '../../enums.js';

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
  withIsMultiSelect(isMultiSelect = true): this { this.input.isMultiSelect = isMultiSelect; return this; }
  /** Sets whether choice labels wrap. @param wrap True to allow wrapping. @returns The builder instance for method chaining. */
  withWrap(wrap = true): this { this.input.wrap = wrap; return this; }
  /** Sets whether a selection is required. @param isRequired True if required. @returns The builder instance for method chaining. */
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  /** Sets the error message shown when validation fails. @param errorMessage The error message. @returns The builder instance for method chaining. */
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.input.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.input.isVisible = isVisible; return this; }
  /** Sets the height of the element. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.input.height = height; return this; }
  /** Sets the fallback behavior when the element is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.input.fallback = fallback; return this; }
  /** Sets the feature requirements for the element. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.input.requires = { ...this.input.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.input.rtl = rtl; return this; }
  /** Sets the label position relative to the input. @param position The label position. @returns The builder instance for method chaining. */
  withLabelPosition(position: InputLabelPosition): this { this.input.labelPosition = position; return this; }
  /** Sets the width of the label. @param width The label width (e.g. '100px' or '30%'). @returns The builder instance for method chaining. */
  withLabelWidth(width: string): this { this.input.labelWidth = width; return this; }
  /** Sets the visual style of the input. @param style The input style. @returns The builder instance for method chaining. */
  withInputStyle(style: InputStyle): this { this.input.inputStyle = style; return this; }

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
