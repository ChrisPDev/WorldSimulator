﻿using System;
using System.Globalization;
using System.Windows.Data;
using WorldSimulator.Config;
using System.Diagnostics;

namespace WorldSimulator.Converters
{
    public class CellToCanvasXConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int x)
            {
                return (double)(x * SimulationConfig.CellSize);
            }

            return 0.0;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
