// Copyright(c) Microsoft Corporation.
// This content is subject to the Microsoft Reference Source License,
// see http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Xml;
using System.Xml.XPath;
using Microsoft.HealthVault.Helpers;
using Microsoft.HealthVault.Thing;

namespace Microsoft.HealthVault.ItemTypes
{
    /// <summary>
    /// Represents a thing type that encapsulates an insulin injection.
    /// </summary>
    ///
    /// <remarks>
    /// This class represents any injection unit used to dispense insulin.
    /// The injector might or might not have a device component.
    /// </remarks>
    ///
    public class InsulinInjection : ThingBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="InsulinInjection"/> class with
        /// default values.
        /// </summary>
        ///
        /// <remarks>
        /// The item is not added to the health record until the
        /// <see cref="HealthRecordAccessor.NewItem(ThingBase)"/> method
        /// is called.
        /// </remarks>
        ///
        public InsulinInjection()
            : base(TypeId)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="InsulinInjection"/> class
        /// with the specified insulin type and amount.
        /// </summary>
        ///
        /// <param name="insulinType">
        /// The type of insulin being used.
        /// </param>
        ///
        /// <param name="amount">
        /// The amount of insulin.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="amount"/> or <paramref name="insulinType"/> parameter
        /// is <b>null</b>.
        /// </exception>
        ///
        public InsulinInjection(
            CodableValue insulinType,
            InsulinInjectionMeasurement amount)
            : base(TypeId)
        {
            this.InsulinType = insulinType;
            this.Amount = amount;
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
            new Guid("3B3C053B-B1FE-4E11-9E22-D4B480DE74E8");

        /// <summary>
        /// Populates this <see cref="InsulinInjection"/> instance from the data in the XML.
        /// </summary>
        ///
        /// <param name="typeSpecificXml">
        /// The XML to get the insulin injection data from.
        /// </param>
        ///
        /// <exception cref="InvalidOperationException">
        /// The first node in <paramref name="typeSpecificXml"/> is not
        /// an insulin-injection node.
        /// </exception>
        ///
        protected override void ParseXml(IXPathNavigable typeSpecificXml)
        {
            XPathNavigator itemNav =
                typeSpecificXml.CreateNavigator().SelectSingleNode(
                    "insulin-injection");

            Validator.ThrowInvalidIfNull(itemNav, Resources.InsulinInjectionUnexpectedNode);

            this.insulinType = new CodableValue();
            this.insulinType.ParseXml(itemNav.SelectSingleNode("type"));

            this.amount = new InsulinInjectionMeasurement();
            this.amount.ParseXml(itemNav.SelectSingleNode("amount"));

            XPathNavigator deviceIdNav =
                itemNav.SelectSingleNode("device-id");

            if (deviceIdNav != null)
            {
                this.deviceId = deviceIdNav.Value;
            }
        }

        /// <summary>
        /// Writes the insulin injection data to the specified XmlWriter.
        /// </summary>
        ///
        /// <param name="writer">
        /// The XmlWriter to write the insulin injection data to.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="writer"/> is <b>null</b>.
        /// </exception>
        ///
        /// <exception cref="HealthRecordItemSerializationException">
        /// The <see cref="InsulinType"/> property is <b>null</b>, or the
        /// <see cref="Amount"/> property is <b>null</b> or empty.
        /// </exception>
        ///
        public override void WriteXml(XmlWriter writer)
        {
            Validator.ThrowIfWriterNull(writer);
            Validator.ThrowSerializationIfNull(this.insulinType, Resources.InsulinInjectionTypeNotSet);
            Validator.ThrowSerializationIfNull(this.amount, Resources.InsulinInjectionAmountNotSet);

            // <insulin-injection>
            writer.WriteStartElement("insulin-injection");

            // <type>
            this.insulinType.WriteXml("type", writer);

            // <amount>
            this.amount.WriteXml("amount", writer);

            if (!string.IsNullOrEmpty(this.deviceId))
            {
                // <device-id>
                writer.WriteElementString("device-id", this.deviceId);
            }

            // </insulin-injection>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets or sets the type of insulin being used in the injector.
        /// </summary>
        ///
        /// <value>
        /// A <see cref="CodableValue"/> representing the type.
        /// </value>
        ///
        /// <remarks>
        /// The preferred vocabulary is "insulin-types".
        /// </remarks>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is <b>null</b>.
        /// </exception>
        ///
        public CodableValue InsulinType
        {
            get { return this.insulinType; }

            set
            {
                Validator.ThrowIfArgumentNull(value, nameof(InsulinType), Resources.InsulinInjectionTypeMandatory);
                this.insulinType = value;
            }
        }

        private CodableValue insulinType;

        /// <summary>
        /// Gets or sets the amount of insulin.
        /// </summary>
        ///
        /// <value>
        /// An <see cref="InsulinInjectionMeasurement"/> representing
        /// the amount.
        /// </value>
        ///
        /// <exception cref="ArgumentNullException">
        /// The value is <b>null</b>.
        /// </exception>
        ///
        public InsulinInjectionMeasurement Amount
        {
            get { return this.amount; }

            set
            {
                Validator.ThrowIfArgumentNull(value, nameof(Amount), Resources.InsulinInjectionAmountMandatory);
                this.amount = value;
            }
        }

        private InsulinInjectionMeasurement amount;

        /// <summary>
        /// Gets or sets the identifier for the device.
        /// </summary>
        ///
        /// <value>
        /// A string representing the identifier.
        /// </value>
        ///
        /// <remarks>
        /// Set the value to <b>null</b> if the device identifier should not be
        /// stored.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="value"/> contains only whitespace.
        /// </exception>
        ///
        public string DeviceId
        {
            get { return this.deviceId; }

            set
            {
                Validator.ThrowIfStringIsWhitespace(value, "DeviceId");
                this.deviceId = value;
            }
        }

        private string deviceId;

        /// <summary>
        /// Gets a string representation of the insulin injection item.
        /// </summary>
        ///
        /// <returns>
        /// A string representation of the insulin injection item.
        /// </returns>
        ///
        public override string ToString()
        {
            return
                string.Format(
                    Resources.InsulinInjectionToStringFormat,
                    this.InsulinType.Text,
                    this.Amount.ToString());
        }
    }
}