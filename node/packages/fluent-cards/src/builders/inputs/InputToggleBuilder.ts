import { InputLabelPosition, InputStyle, Spacing } from '../../enums.js';
import type { InputToggle, AdaptiveElement } from '../../models.js';

/** Fluent builder for {@link InputToggle} elements. */
export class InputToggleBuilder {
  private readonly input: InputToggle = { type: 'Input.Toggle', id: '', title: '' };

  /** Sets the unique identifier (required for form submission). @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.input.id = id; return this; }
  /** Sets the required toggle label displayed beside the checkbox. @param title The toggle label. @returns The builder instance for method chaining. */
  withTitle(title: string): this { this.input.title = title; return this; }
  /** Sets the additional descriptive label displayed above the toggle. @param label The label text. @returns The builder instance for method chaining. */
  withLabel(label: string): this { this.input.label = label; return this; }
  /** Sets the initial value of the toggle. @param value The initial value. @returns The builder instance for method chaining. */
  withValue(value: string): this { this.input.value = value; return this; }
  /** Sets the value submitted when the toggle is on. @param valueOn The on value. @returns The builder instance for method chaining. */
  withValueOn(valueOn: string): this { this.input.valueOn = valueOn; return this; }
  /** Sets the value submitted when the toggle is off. @param valueOff The off value. @returns The builder instance for method chaining. */
  withValueOff(valueOff: string): this { this.input.valueOff = valueOff; return this; }
  /** Sets whether the toggle label wraps. @param wrap True to allow wrapping. @returns The builder instance for method chaining. */
  withWrap(wrap = true): this { this.input.wrap = wrap; return this; }
  /** Sets whether a value is required. @param isRequired True if required. @returns The builder instance for method chaining. */
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

  /** Builds and returns the configured InputToggle element. @returns The configured InputToggle instance. */
  build(): InputToggle { return this.input; }
}
