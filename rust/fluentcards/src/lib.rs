//! # FluentCards
//!
//! A fluent builder library for creating [Adaptive Cards](https://adaptivecards.io)
//! conforming to the 1.6.0 specification.
//!
//! # Quick Start
//!
//! ```rust
//! use fluent_cards::*;
//!
//! let card = AdaptiveCardBuilder::new()
//!     .with_version("1.5")
//!     .add_text_block(|tb| {
//!         tb.with_text("Hello, FluentCards!")
//!           .with_size(TextSize::Large)
//!           .with_weight(TextWeight::Bolder)
//!           .with_wrap(true);
//!     })
//!     .add_action(|a| {
//!         a.open_url("https://adaptivecards.io")
//!          .with_title("Learn More");
//!     })
//!     .build();
//!
//! let json = to_json(&card).unwrap();
//! println!("{json}");
//! ```

mod models;
mod enums;
pub mod builders;
mod serialization;
mod validation;
mod teams;

pub use models::*;
pub use enums::*;
pub use builders::*;
pub use serialization::*;
pub use validation::*;
pub use teams::*;
