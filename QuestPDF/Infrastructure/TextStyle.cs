﻿using System;
using HarfBuzzSharp;
using QuestPDF.Helpers;

namespace QuestPDF.Infrastructure
{
    public class TextStyle
    {
        internal bool HasGlobalStyleApplied { get; private set; }
        
        internal string? Color { get; set; }
        internal string? BackgroundColor { get; set; }
        internal string? FontFamily { get; set; }
        internal float? Size { get; set; }
        internal float? LineHeight { get; set; }
        internal FontWeight? FontWeight { get; set; }
        internal FontPosition? FontPosition { get; set; }
        internal bool? IsItalic { get; set; }
        internal bool? HasStrikethrough { get; set; }
        internal bool? HasUnderline { get; set; }
        internal bool? WrapAnywhere { get; set; }

        internal TextStyle? Fallback { get; set; }
        
        // TODO: without cache, this may be an expensive operation
        internal object PaintKey => (FontFamily, Size, FontWeight, FontPosition, IsItalic, Color);
        internal object FontMetricsKey => (FontFamily, Size, FontWeight, IsItalic);
        
        internal static readonly TextStyle LibraryDefault = new TextStyle
        {
            Color = Colors.Black,
            BackgroundColor = Colors.Transparent,
            FontFamily = Fonts.Lato,
            Size = 12,
            LineHeight = 1.2f,
            FontWeight = Infrastructure.FontWeight.Normal,
            FontPosition = Infrastructure.FontPosition.Normal,
            IsItalic = false,
            HasStrikethrough = false,
            HasUnderline = false,
            WrapAnywhere = false,
            Fallback = null
        };

        // it is important to create new instances for the DefaultTextStyle element to work correctly
        public static TextStyle Default => new TextStyle();
        
        internal void ApplyGlobalStyle(TextStyle globalStyle)
        {
            if (HasGlobalStyleApplied)
                return;
            
            HasGlobalStyleApplied = true;
            ApplyParentStyle(globalStyle);

            if (Fallback != null)
                ApplyFallbackStyle(this);
        }

        internal void ApplyFallbackStyle(TextStyle parentStyle)
        {
            ApplyParentStyle(parentStyle, false);
            Fallback?.ApplyFallbackStyle(this);
        }
        
        internal void ApplyParentStyle(TextStyle parentStyle, bool mapFallback = true)
        {
            Color ??= parentStyle.Color;
            BackgroundColor ??= parentStyle.BackgroundColor;
            FontFamily ??= parentStyle.FontFamily;
            Size ??= parentStyle.Size;
            LineHeight ??= parentStyle.LineHeight;
            FontWeight ??= parentStyle.FontWeight;
            FontPosition ??= parentStyle.FontPosition;
            IsItalic ??= parentStyle.IsItalic;
            HasStrikethrough ??= parentStyle.HasStrikethrough;
            HasUnderline ??= parentStyle.HasUnderline;
            WrapAnywhere ??= parentStyle.WrapAnywhere;
            
            if (mapFallback)
                Fallback ??= parentStyle.Fallback?.Clone();
        }

        internal void OverrideStyle(TextStyle parentStyle)
        {
            Color = parentStyle.Color ?? Color;
            BackgroundColor = parentStyle.BackgroundColor ?? BackgroundColor;
            FontFamily = parentStyle.FontFamily ?? FontFamily;
            Size = parentStyle.Size ?? Size;
            LineHeight = parentStyle.LineHeight ?? LineHeight;
            FontWeight = parentStyle.FontWeight ?? FontWeight;
            FontPosition = parentStyle.FontPosition ?? FontPosition;
            IsItalic = parentStyle.IsItalic ?? IsItalic;
            HasStrikethrough = parentStyle.HasStrikethrough ?? HasStrikethrough;
            HasUnderline = parentStyle.HasUnderline ?? HasUnderline;
            WrapAnywhere = parentStyle.WrapAnywhere ?? WrapAnywhere;
            Fallback = parentStyle.Fallback?.Clone() ?? Fallback;
        }
        
        internal TextStyle Clone()
        {
            var clone = (TextStyle)MemberwiseClone();
            clone.HasGlobalStyleApplied = false;
            clone.Fallback = Fallback?.Clone();
            return clone;
        }
    }
}