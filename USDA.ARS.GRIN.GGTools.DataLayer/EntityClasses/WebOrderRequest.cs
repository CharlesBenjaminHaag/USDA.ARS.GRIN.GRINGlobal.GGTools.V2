using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class WebOrderRequest : AppEntityBase
    {
        public int WebCooperatorID { get; set; }
        public int WebUserID { get; set; }
        public string WebCooperatorLastName { get; set; }
        public string WebCooperatorTitle { get; set; }
        public string WebCooperatorFirstName { get; set; }
        public string WebCooperatorFullName { get; set; }
        public string WebCooperatorPrimaryPhone { get; set; }
        public string WebCooperatorEmail { get; set; }
        public string WebCooperatorOrganization { get; set; }
        public DateTime WebCooperatorCreatedDate { get; set; }
        public string WebCooperatorVettedStatusCode { get; set; }
        public string WebCooperatorAddress1  { get; set; }
        public string WebCooperatorAddress2  { get; set; }
        public string WebCooperatorAddress3  { get; set; }
        public string WebCooperatorAddressCity  { get; set; }
        public string WebCooperatorAddressPostalIndex  { get; set; }
        public string WebCooperatorAddressState  { get; set; }
        public string WebCooperatorAddressCountry { get; set; }
        public string WebCooperatorAddressCountryDescription { get; set; }
        public string ShippingAddress1  { get; set; }
        public string ShippingAddress2  { get; set; }
        public string ShippingAddress3  { get; set; }
        public string ShippingAddressCity  { get; set; }
        public string ShippingAddressPostalIndex  { get; set; }
        public string ShippingAddressState  { get; set; }
        public string ShippingAddressCountryCode { get; set; }
        public string ShippingAddressCountryDescription { get; set; }

        public string ShippingAddressFormatted
        {
            get
            {
                return GetFormattedAddress(ShippingAddress1, ShippingAddress2, ShippingAddress3, ShippingAddressCity, ShippingAddressState, ShippingAddressPostalIndex, ShippingAddressCountryCode, ShippingAddressCountryDescription);
            }
        }

        public string WebCooperatorAddressFormatted
        {
            get
            {
                return GetFormattedAddress(WebCooperatorAddress1, WebCooperatorAddress2, WebCooperatorAddress3, WebCooperatorAddressCity, WebCooperatorAddressState, WebCooperatorAddressPostalIndex, WebCooperatorAddressCountry, WebCooperatorAddressCountryDescription);
            }
        }

        public DateTime OrderDate { get; set; }
        public string IntendedUseCode { get; set; }
        public string IntendedUseNote { get; set; }
        public string StatusCode { get; set; }
        public string StatusTitle { get; set; }
        public string MostRecentOrderAction { get; set; }
        public string MostRecentWebOrderAction { get; set; }
        public string IsPreviouslyNRRReviewed { get; set; }
        public string SpecialInstruction { get; set; }
        public string EmailAddressList { get; set; }
        public Cooperator WebCooperator { get; set; }
        public List<Cooperator> Cooperators { get; set; }
        public List<Address> Addresses { get; set; }
        public int TotalGenera { get; set; }
        public int TotalItems { get; set; }
        public int TotalSites { get; set; }
        public string CSSClass
        {
            get
            {
                switch (StatusCode)
                {
                    case "NRR_FLAG":
                        return "bg-red";
                    case "CANCELED":
                        return "bg-yellow";
                    case "ACCEPTED":
                        return "bg-green";
                    case "SUBMITTED":
                        return "bg-purple";
                    case "NRR_REVIEW":
                        return "bg-gray";
                    default:
                        return "";
                }
            }
        }
        public bool IsLocked { get; set; }
        public string IsOnHold { get; set; }
        public string IsOnCountryHold { get; set; }
        public int RelatedOrders { get; set; }
        public int OwnedByWebUserID { get; set; }
        public string OwnedByWebCooperatorName { get; set; }
        public DateTime OwnedByDate { get; set; }

        public string GetFormattedAddress(string add1, string add2, string add3,string cityOrProvince, string stateOrProvince, string postalCode, string countryCode, string CountryDescription)
        {
            var addressParts = new List<string>();

            // Add address lines if they are not null or empty
            if (!string.IsNullOrEmpty(ShippingAddress1))
            {
                addressParts.Add(add1);
            }
            if (!string.IsNullOrEmpty(add2))
            {
                addressParts.Add(add2);
            }
            if (!string.IsNullOrEmpty(add3))
            {
                addressParts.Add(add3);
            }

            // Add city and state or province (only for US format)
            if (countryCode == "USA")
            {
                if (!string.IsNullOrEmpty(cityOrProvince))
                {
                    addressParts.Add($"{cityOrProvince}, {stateOrProvince} {postalCode}");
                }
                else
                {
                    addressParts.Add($"{stateOrProvince} {postalCode}");
                }
            }
            else
            {
                // For international addresses, add city, state/province, postal code, and countryCode
                if (!string.IsNullOrEmpty(cityOrProvince))
                {
                    addressParts.Add(cityOrProvince);
                }
                if (!string.IsNullOrEmpty(stateOrProvince))
                {
                    addressParts.Add(stateOrProvince);
                }
                if (!string.IsNullOrEmpty(postalCode))
                {
                    addressParts.Add(postalCode);
                }
                
            }

            if (!string.IsNullOrEmpty(CountryDescription))
            {
                addressParts.Add(CountryDescription);
            }

            // Join the address parts with newlines and return the formatted address
            return $"<div>{string.Join("<br>", addressParts)}</div>";
        }
    }
}
