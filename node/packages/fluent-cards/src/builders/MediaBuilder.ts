import { Spacing } from '../enums.js';
import type { Media, MediaSource, AdaptiveElement } from '../models.js';

/** Fluent builder for {@link Media} elements. */
export class MediaBuilder {
  private readonly media: Media = { type: 'Media', sources: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.media.id = id; return this; }
  /** Sets the poster image URL displayed before playback. @param poster The poster URL. @returns The builder instance for method chaining. */
  withPoster(poster: string): this { this.media.poster = poster; return this; }
  /** Sets the alternate text for accessibility. @param altText The alt text. @returns The builder instance for method chaining. */
  withAltText(altText: string): this { this.media.altText = altText; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.media.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.media.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.media.isVisible = isVisible; return this; }
  /** Sets the height of the element. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.media.height = height; return this; }
  /** Sets the fallback behavior when the element is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.media.fallback = fallback; return this; }
  /** Sets the feature requirements for the element. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.media.requires = { ...this.media.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.media.rtl = rtl; return this; }

  /** Adds a media source with a URL and MIME type. @param url The media URL. @param mimeType The MIME type of the source. @returns The builder instance for method chaining. */
  addSource(url: string, mimeType: string): this;
  /** Adds a pre-built MediaSource object. @param source A pre-built MediaSource. @returns The builder instance for method chaining. */
  addSource(source: MediaSource): this;
  addSource(urlOrSource: string | MediaSource, mimeType?: string): this {
    if (typeof urlOrSource === 'string') {
      this.media.sources!.push({ url: urlOrSource, mimeType });
    } else {
      this.media.sources!.push(urlOrSource);
    }
    return this;
  }

  /** Builds and returns the configured Media element. @returns The configured Media instance. */
  build(): Media { return this.media; }
}
