﻿using Microsoft.Maui.Graphics;
namespace Gtk.UIExtensions.Common
{
    /// <summary>
    /// Interface of the controls which can measure their size taking into
    /// account the available area.
    /// </summary>
    public interface IMeasurable
    {
        /// <summary>
        /// Measures the size of the control in order to fit it into the
        /// available area.
        /// </summary>
        /// <param name="availableWidth">Available width.</param>
        /// <param name="availableHeight">Available height.</param>
        /// <returns>Size of the control that fits the available area.</returns>
        Size Measure(double availableWidth, double availableHeight);
    }
}
