// Copyright (c) Microsoft Corporation.  All rights reserved.
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Microsoft.HealthVault.Clients;
using Microsoft.HealthVault.Helpers;
using Microsoft.HealthVault.Thing;

namespace Microsoft.HealthVault.ItemTypes
{
    /// <summary>
    /// Represents a thing type that encapsulates a person's
    /// respiratory profile at a single point in time.
    /// </summary>
    ///
    public class RespiratoryProfile : ThingBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="RespiratoryProfile"/> class with default values.
        /// </summary>
        ///
        /// <remarks>
        /// The item is not added to the health record until the <see cref="IThingClient.CreateNewThingsAsync{ThingBase}(Guid, ICollection{ThingBase})"/> method is called.
        /// </remarks>
        ///
        public RespiratoryProfile()
            : base(TypeId)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RespiratoryProfile"/> class
        /// with the specified date and time.
        /// </summary>
        ///
        /// <param name="when">
        /// The date/time when the respiratory profile was take.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="when"/> parameter is <b>null</b>.
        /// </exception>
        ///
        public RespiratoryProfile(HealthServiceDateTime when)
            : base(TypeId)
        {
            When = when;
        }

        /// <summary>
        /// Retrieves the unique identifier for the item type.
        /// </summary>
        ///
        /// <value>
        /// A GUID.
        /// </value>
        ///
        public static new readonly Guid TypeId =
            new Guid("5fd15cb7-b717-4b1c-89e0-1dbcf7f815dd");

        /// <summary>
        /// Populates this <see cref="RespiratoryProfile"/> instance from the data in the XML.
        /// </summary>
        ///
        /// <param name="typeSpecificXml">
        /// The XML to get the respiratory profile data from.
        /// </param>
        ///
        /// <exception cref="InvalidOperationException">
        /// The first node in <paramref name="typeSpecificXml"/> is not
        /// a respiratory-profile node.
        /// </exception>
        ///
        protected override void ParseXml(IXPathNavigable typeSpecificXml)
        {
            XPathNavigator itemNav =
                typeSpecificXml.CreateNavigator().SelectSingleNode(
                    "respiratory-profile");

            Validator.ThrowInvalidIfNull(itemNav, Resources.RespiratoryProfileUnexpectedNode);

            _when = new HealthServiceDateTime();
            _when.ParseXml(itemNav.SelectSingleNode("when"));

            _expiratoryFlowRedZoneUpperBoundary =
                XPathHelper.GetOptNavValue<FlowMeasurement>(
                    itemNav,
                    "expiratory-flow-red-zone-upper-boundary");

            _expiratoryFlowOrangeZoneUpperBoundary =
                XPathHelper.GetOptNavValue<FlowMeasurement>(
                    itemNav,
                    "expiratory-flow-orange-zone-upper-boundary");

            _expiratoryFlowYellowZoneUpperBoundary =
                XPathHelper.GetOptNavValue<FlowMeasurement>(
                    itemNav,
                    "expiratory-flow-yellow-zone-upper-boundary");
        }

        /// <summary>
        /// Writes the respiratory profile data to the specified XmlWriter.
        /// </summary>
        ///
        /// <param name="writer">
        /// The XmlWriter to write the respiratory profile data to.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="writer"/> is <b>null</b>.
        /// </exception>
        ///
        public override void WriteXml(XmlWriter writer)
        {
            Validator.ThrowIfWriterNull(writer);

            // <respiratory-profile>
            writer.WriteStartElement("respiratory-profile");

            // <when>
            _when.WriteXml("when", writer);

            XmlWriterHelper.WriteOpt(
                writer,
                "expiratory-flow-red-zone-upper-boundary",
                _expiratoryFlowRedZoneUpperBoundary);

            XmlWriterHelper.WriteOpt(
                writer,
                "expiratory-flow-orange-zone-upper-boundary",
                _expiratoryFlowOrangeZoneUpperBoundary);

            XmlWriterHelper.WriteOpt(
                writer,
                "expiratory-flow-yellow-zone-upper-boundary",
                _expiratoryFlowYellowZoneUpperBoundary);

            // </respiratory-profile>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets or sets the date/time when the respiratory profile was taken.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="HealthServiceDateTime"/> representing the date.
        /// The default value is the current year, month, and day.
        /// </value>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is <b>null</b>.
        /// </exception>
        ///
        public HealthServiceDateTime When
        {
            get { return _when; }

            set
            {
                Validator.ThrowIfArgumentNull(value, nameof(When), Resources.WhenNullValue);
                _when = value;
            }
        }

        private HealthServiceDateTime _when = new HealthServiceDateTime();

        /// <summary>
        /// Gets or sets the upper boundary of the expiratory flow red zone.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="FlowMeasurement"/> representing the upper boundary
        /// of the expiratory flow in L/s.
        /// </value>
        ///
        /// <remarks>
        /// The red zone covers flow measurements from 0 to the red zone upper
        /// boundary. Values are measured in liters per second (L/s).
        /// </remarks>
        ///
        public FlowMeasurement ExpiratoryFlowRedZoneUpperBoundary
        {
            get { return _expiratoryFlowRedZoneUpperBoundary; }
            set { _expiratoryFlowRedZoneUpperBoundary = value; }
        }

        private FlowMeasurement _expiratoryFlowRedZoneUpperBoundary;

        /// <summary>
        /// Gets or sets the upper boundary of the expiratory flow orange zone.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="FlowMeasurement"/> representing the upper boundary
        /// of the expiratory flow in L/s.
        /// </value>
        ///
        /// <remarks>
        /// The orange zone covers flow measurements from the red zone upper boundary
        /// to the orange zone upper boundary.
        /// Values are measured in liters per second (L/s).
        /// </remarks>
        ///
        public FlowMeasurement ExpiratoryFlowOrangeZoneUpperBoundary
        {
            get { return _expiratoryFlowOrangeZoneUpperBoundary; }
            set { _expiratoryFlowOrangeZoneUpperBoundary = value; }
        }

        private FlowMeasurement _expiratoryFlowOrangeZoneUpperBoundary;

        /// <summary>
        /// Gets or sets the upper boundary of the expiratory flow yellow zone.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="FlowMeasurement"/> representing the upper boundary
        /// of the expiratory flow in L/s.
        /// </value>
        ///
        /// <remarks>
        /// The yellow zone covers flow measurements from the orange zone upper boundary
        /// to the yellow zone upper boundary.
        /// Values are measured in liters per second (L/s).
        /// </remarks>
        ///
        public FlowMeasurement ExpiratoryFlowYellowZoneUpperBoundary
        {
            get { return _expiratoryFlowYellowZoneUpperBoundary; }
            set { _expiratoryFlowYellowZoneUpperBoundary = value; }
        }

        private FlowMeasurement _expiratoryFlowYellowZoneUpperBoundary;

        /// <summary>
        /// Gets a string representation of the respiratory profile.
        /// </summary>
        ///
        /// <returns>
        /// A string representing the respiratory profile.
        /// </returns>
        ///
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(250);
            if (ExpiratoryFlowRedZoneUpperBoundary != null)
            {
                result.AppendFormat(
                    Resources.RespiratoryProfileToStringEFRedZoneUpper,
                    ExpiratoryFlowRedZoneUpperBoundary.ToString());
            }

            if (ExpiratoryFlowOrangeZoneUpperBoundary != null)
            {
                if (ExpiratoryFlowRedZoneUpperBoundary != null)
                {
                    result.AppendFormat(
                        Resources.ListSeparator);
                }

                if (ExpiratoryFlowRedZoneUpperBoundary != null)
                {
                    result.AppendFormat(
                        Resources.RespiratoryProfileToStringEFOrangeZone,
                        ExpiratoryFlowRedZoneUpperBoundary.ToString(),
                        ExpiratoryFlowOrangeZoneUpperBoundary.ToString());
                }
                else
                {
                    result.AppendFormat(
                        Resources.RespiratoryProfileToStringEFOrangeZoneUpper,
                        ExpiratoryFlowOrangeZoneUpperBoundary.ToString());
                }
            }

            if (ExpiratoryFlowYellowZoneUpperBoundary != null)
            {
                if (ExpiratoryFlowRedZoneUpperBoundary != null ||
                    ExpiratoryFlowOrangeZoneUpperBoundary != null)
                {
                    result.AppendFormat(
                        Resources.ListSeparator);
                }

                if (ExpiratoryFlowOrangeZoneUpperBoundary != null)
                {
                    result.AppendFormat(
                        Resources.RespiratoryProfileToStringEFYellowZone,
                        ExpiratoryFlowOrangeZoneUpperBoundary.ToString(),
                        ExpiratoryFlowYellowZoneUpperBoundary.ToString());
                }
                else
                {
                    result.AppendFormat(
                        Resources.RespiratoryProfileToStringEFYellowZoneUpper,
                        ExpiratoryFlowYellowZoneUpperBoundary.ToString());
                }
            }

            return result.ToString();
        }
    }
}
