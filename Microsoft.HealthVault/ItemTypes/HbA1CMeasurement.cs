// Copyright (c) Microsoft Corporation.  All rights reserved.
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.HealthVault.ItemTypes
{
    /// <summary>
    /// Represents the result of an HBA1C assay in mmol/mol.
    /// </summary>
    ///
    /// <remarks>
    /// Represents HBA1C results using the International Federation of Clinical Chemistry and
    /// Laboratory Medicine (IFCC) standard units of millimoles per mole of unglycated
    /// hemoglobin in the blood.
    /// </remarks>
    ///
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1709:IdentifiersShouldBeCasedCorrectly",
        Justification = "Hb is the correct capitalization here.")]
    public class HbA1CMeasurement : Measurement<double>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="HbA1CMeasurement"/>
        /// class with empty values.
        /// </summary>
        ///
        public HbA1CMeasurement()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HbA1CMeasurement"/>
        /// class with the specified value in millimoles per mole (mmol/mol).
        /// </summary>
        ///
        /// <param name="millimolesPerMole">
        /// The concentration of unglycated hemoglobin in the blood in millimoles per mole.
        /// </param>
        ///
        public HbA1CMeasurement(double millimolesPerMole)
            : base(millimolesPerMole)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HbA1CMeasurement"/>
        /// class with the specified value in millimoles per mole (mmol/mol)
        /// and display value.
        /// </summary>
        ///
        /// <param name="millimolesPerMole">
        /// The concentration value in millimoles per mole.
        /// </param>
        ///
        /// <param name="displayValue">
        /// The display value of the HbA1C measurement. This should
        /// contain the exact measurement as entered by the user, even if it
        /// uses some other unit of measure besides mmol/mol. The display value
        /// <see cref="DisplayValue.Units"/> and
        /// <see cref="DisplayValue.UnitsCode"/>
        /// represents the unit of measure for the user-entered value.
        /// </param>
        ///
        public HbA1CMeasurement(
            double millimolesPerMole,
            DisplayValue displayValue)
            : base(millimolesPerMole, displayValue)
        {
        }

        /// <summary>
        /// Verifies that the value is a legal HbA1C value.
        /// </summary>
        ///
        /// <param name="value">
        /// The HbA1C measurement.
        /// </param>
        ///
        protected override void AssertMeasurementValue(double value)
        {
        }

        /// <summary>
        /// Populates the data for the HbA1C value from the XML.
        /// </summary>
        ///
        /// <param name="navigator">
        /// The XML node representing the HbA1C value.
        /// </param>
        ///
        protected override void ParseValueXml(XPathNavigator navigator)
        {
            Value = navigator.SelectSingleNode("mmol-per-mol").ValueAsDouble;
        }

        /// <summary>
        /// Writes the HbA1C value to the specified XML writer.
        /// </summary>
        ///
        /// <param name="writer">
        /// The XmlWriter to write the HbA1C value to.
        /// </param>
        ///
        protected override void WriteValueXml(XmlWriter writer)
        {
            writer.WriteElementString(
                "mmol-per-mol", XmlConvert.ToString(Value));
        }

        /// <summary>
        /// Gets a string representation of the HbA1C value in the base units.
        /// </summary>
        /// <returns>
        /// The HbA1C value as a string in the base units.
        /// </returns>
        ///
        protected override string GetValueString(double value)
        {
            return value.ToString(CultureInfo.CurrentCulture);
        }
    }
}