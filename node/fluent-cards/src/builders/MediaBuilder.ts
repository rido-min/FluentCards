import { Spacing } from '../enums.js';
import type { Media, MediaSource } from '../models.js';

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

  /** Adds a media source by URL and MIME type, or a pre-built MediaSource object. @param urlOrSource The URL string or a pre-built MediaSource. @param mimeType The MIME type (required when URL is provided). @returns The builder instance for method chaining. */
  addSource(url: string, mimeType: string): this;
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
