---
name: Culinare Design System
colors:
  surface: '#fff8f6'
  surface-dim: '#edd5cb'
  surface-bright: '#fff8f6'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#fff1eb'
  surface-container: '#ffeae0'
  surface-container-high: '#fce3d9'
  surface-container-highest: '#f6ded3'
  on-surface: '#251913'
  on-surface-variant: '#584237'
  inverse-surface: '#3c2d26'
  inverse-on-surface: '#ffede6'
  outline: '#8c7164'
  outline-variant: '#e0c0b1'
  surface-tint: '#9d4300'
  primary: '#9d4300'
  on-primary: '#ffffff'
  primary-container: '#f97316'
  on-primary-container: '#582200'
  inverse-primary: '#ffb690'
  secondary: '#0058be'
  on-secondary: '#ffffff'
  secondary-container: '#2170e4'
  on-secondary-container: '#fefcff'
  tertiary: '#006398'
  on-tertiary: '#ffffff'
  tertiary-container: '#00a2f4'
  on-tertiary-container: '#003554'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#ffdbca'
  primary-fixed-dim: '#ffb690'
  on-primary-fixed: '#341100'
  on-primary-fixed-variant: '#783200'
  secondary-fixed: '#d8e2ff'
  secondary-fixed-dim: '#adc6ff'
  on-secondary-fixed: '#001a42'
  on-secondary-fixed-variant: '#004395'
  tertiary-fixed: '#cde5ff'
  tertiary-fixed-dim: '#93ccff'
  on-tertiary-fixed: '#001d32'
  on-tertiary-fixed-variant: '#004b74'
  background: '#fff8f6'
  on-background: '#251913'
  surface-variant: '#f6ded3'
typography:
  display-xl:
    fontFamily: Inter
    fontSize: 48px
    fontWeight: '700'
    lineHeight: 56px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Inter
    fontSize: 32px
    fontWeight: '700'
    lineHeight: 40px
    letterSpacing: -0.01em
  headline-md:
    fontFamily: Inter
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  body-lg:
    fontFamily: Inter
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 28px
  body-md:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  label-md:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
  caption:
    fontFamily: Inter
    fontSize: 12px
    fontWeight: '400'
    lineHeight: 16px
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  base: 8px
  xs: 4px
  sm: 12px
  md: 24px
  lg: 48px
  xl: 80px
  container-max: 1280px
  gutter: 24px
---

## Brand & Style

This design system is built to evoke a sense of freshness, warmth, and immediate craving. The brand personality is "The Professional Chef Next Door"—expert and reliable, yet accessible and inviting. 

We utilize a **Modern Minimalism** style with high-quality imagery and significant whitespace to allow food photography to take center stage. The interface avoids unnecessary clutter, focusing on high legibility and intuitive task completion. By combining clean layout principles with warm, appetizing accents, the design system ensures users feel both hungry for the content and confident in the service.

## Colors

The palette is anchored by a warm, energetic orange that stimulates appetite and signals action. 

- **Primary**: Used for key CTAs, brand highlights, and active states.
- **Surface**: The application relies on a clean white base for cards and high-level surfaces, with a very light gray (#F9FAFB) for page backgrounds to provide subtle contrast.
- **Typography**: A deep dark gray (#111827) is used instead of pure black to maintain a sophisticated, softer feel while ensuring maximum readability.
- **Semantic Accents**: Colors are strictly functional. Green indicates availability or success; Red signals errors or stock depletion; Amber manages the "pending" state; and Blue is reserved for financial/paid transaction status.

## Typography

This design system utilizes **Inter** exclusively to achieve a modern, utilitarian aesthetic that performs exceptionally well across various screen densities. 

The type hierarchy focuses on clear differentiation through weight and scale. Headlines use a tighter letter-spacing and heavier weights (600-700) to create a bold, editorial feel for dish names and categories. Body text is optimized for legibility with generous line heights. Labels and metadata utilize a medium-to-semibold weight to ensure they remain scannable even at smaller sizes.

## Layout & Spacing

The layout follows a **Fixed Grid** model for desktop, centered within a maximum container width of 1280px to ensure food imagery doesn't become over-extended. 

We use an 8px base grid system to maintain a rhythmic vertical flow. White space is treated as a first-class citizen, particularly around card groups and checkout summaries, to reduce cognitive load during the decision-making process. Margins between major sections should scale between 48px and 80px to create a clear visual break between different cuisines or restaurant categories.

## Elevation & Depth

Hierarchy is established through **Ambient Shadows** and tonal layering. This design system avoids harsh borders in favor of soft, diffused shadows that lift cards off the light gray background.

- **Level 0 (Background)**: #F9FAFB.
- **Level 1 (Cards/Navigation)**: Pure white surface with a subtle shadow (0px 4px 6px -1px rgba(0,0,0,0.05)).
- **Level 2 (Hover/Active)**: Increased shadow depth (0px 10px 15px -3px rgba(0,0,0,0.1)) to provide tactile feedback when a user interacts with a menu item.
- **Level 3 (Modals/Overlays)**: Highest elevation with a semi-transparent backdrop blur to maintain context while focusing on the specific task.

## Shapes

The shape language is consistently **Rounded**, using an 8px (standard) to 12px (large) radius. 

- **8px (Base)**: Applied to input fields, buttons, and small UI elements.
- **12px (Large)**: Reserved for product cards and main container sections to soften the visual impact and make the app feel more approachable.
- **Pill (Full)**: Used exclusively for category badges and tags to distinguish them from actionable buttons.

## Components

### Buttons
- **Primary**: Solid #F97316 with white text. High-contrast, bold, and centered for "Add to Cart" or "Checkout."
- **Secondary**: Ghost style with a #F97316 border and text, used for "View Details" or "Add Instructions."

### Cards
Cards are the primary container for food items. They must feature a top-aligned image with a 12px top-corner radius, followed by padded content sections. Shadow transitions occur on hover to signify interactability.

### Input Fields
Inputs use a white background with a subtle gray border. The **focus state** is high-priority: a 2px stroke of the primary orange color with a soft outer glow to ensure the user always knows where they are typing.

### Badges & Status
Badges use a "soft" color treatment—a light tint of the semantic color for the background with a high-contrast dark version for the text (e.g., Light Green background with Dark Green text for "In-stock").

### Navigation Bar
The top navigation is a fixed, clean white bar with high-contrast text and a subtle bottom shadow. The cart icon should feature a primary orange notification dot for items in the bag.