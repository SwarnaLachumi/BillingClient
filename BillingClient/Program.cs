using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingClient.Service;


namespace BillingClient
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.Write(    "Market Billing System Console Application" +
                              "\n***************************************" 
                              );
            StandardMainTemplate();
            char input = Console.ReadKey().KeyChar;
            ReadinputMain(input);
        }
        private static void ReadinputMain(char input)
        {
            switch (input)
            {
                case '1':
                    Console.Write("\nYou choosed Option 1 - Customer Catologue" +
                                  "\n------------------------------------------");
                    listProductList();
                    StandardMainTemplate();
                    break;
                case '2':
                    Console.Write("\nYou choosed Option 2 - Checkout Customer Basket and generate Receipt" +
                                  "\n-----------------------------------------------------------------------" +
                                  "\nPlease enter the itemID followed by '-' 'Number of Units',separated by comma ',' \nEg:A99-10,B15-2,C20-1" +
                                  "\nIMPORTANT: Provide input in correct format to add item for checkout else input will be ignored in Invalid Itemslist" +
                                  "\n-----------------------------------------------------------------------" +
                                  "\nEnter Items and Units in appropriate format:");
                    string strCheckoutlst = Console.ReadLine();
                    CheckOut(strCheckoutlst);
                    StandardMainTemplate();
                    break;
                case '3':
                    Console.Write("\nYou choosed Option 3 - Update Offer Details" +
                                  "\n-----------------------------------------------------------------------" +
                                  "\nEnter Product ID to update:");
                    string strProductid = Console.ReadLine();
                    Console.Write("\nEnter No of Units to be Offered:");
                    string strOfferUnits = Console.ReadLine();
                    Console.Write("\nEnter Price for offered Units:");
                    string strOfferPrice = Console.ReadLine();
                    UpdateOfferPrice(strProductid, strOfferUnits, strOfferPrice);
                    StandardMainTemplate();
                    break;
                default:
                    Console.Write("Error: Invalid Input Option");
                    StandardMainTemplate();
                    break;
            }
        }
        private static void StandardMainTemplate()
        {
            Console.Write(
                            "\n=>Enter '1' - To visit Products Catalogue" +
                            "\n=>Enter '2' - To checkout Customer Basket and to generate Receipt" +
                            "\n=>Enter '3' - To Update Offer Prices to Products" +
                            "\nEnter Option:"
                            );
            char input = Console.ReadKey().KeyChar;
            ReadinputMain(input);
        }
        private static void listProductList()
        {
            Service.MarketBillingSystemSoapClient ServiceCl = new Service.MarketBillingSystemSoapClient();
            DataTable ProductList = ServiceCl.ListProducts();
            ProductList.TableName = "Product_Details";
            ProductList.Columns.Add("Item_Id");
            ProductList.Columns.Add("Description");
            ProductList.Columns.Add("Unit_Price");
            ProductList.Columns.Add("Sp_Offer_Count");
            ProductList.Columns.Add("Sp_Offer_Price");
            Console.Write("\nCustomer Product Catalogue" +
                          "\n------------------------------------------");
            Console.WriteLine("\nSlNo\t\tItemId\t\tItemDesc\tItemUnitPrice\tOfferUnits\tOfferPrice");
            Console.WriteLine("\n****************************************************************************************\n");

            foreach (DataRow dataRow in ProductList.Rows)
            {
                string row = String.Join("\t\t", dataRow.ItemArray); ;
                Console.WriteLine(row);
            }
            Console.WriteLine("\n");
        }
        
      
        private static void CheckOut(string strItemsList)
        {
            Service.MarketBillingSystemSoapClient ServiceCl = new Service.MarketBillingSystemSoapClient();
            DataTable Receipt = ServiceCl.GetReceipt(strItemsList);
            string GrandTotal = ServiceCl.GetCostGrandTotal(strItemsList);
            string[] ValidItemList = ServiceCl.GetListValidItems(strItemsList);
            string[] InvalidItemsList = ServiceCl.GetListInvalidItems(strItemsList);

            string ValidItemsJoin = string.Join(",", ValidItemList);
            string InvalidItemsListJoin = string.Join(",", InvalidItemsList);
            Console.Write("\n------------------------------------------" +
                           "\nYour Valid Items:" + ValidItemsJoin +
                          "\nYour invalid Items:" + InvalidItemsListJoin+
                           "\n------------------------------------------" );


            Console.Write("\nCustomer Receipt" +
                          "\n------------------------------------------" +
                          "\nSlNo\tItemDescription\t\tUnitPrice\tSpecialOffer\t\tUnits\t\tItem_Cost"+
                          "\n***************************************************************************************************\n");
            foreach (DataRow dataRow in Receipt.Rows)
            {
                string row = String.Join("\t\t", dataRow.ItemArray); ;
                Console.WriteLine(row);
            }

            Console.WriteLine("----------------------------------------------------------------------------------------------------"+
                               "\t\t\t\t\t\t\t\t\t\t\t\tTotal Amount = " + GrandTotal +"\n");
            
        }
        private static void UpdateOfferPrice(string strProductid,string strOfferUnits,string strOfferPrice)
        {
            Service.MarketBillingSystemSoapClient ServiceCl = new Service.MarketBillingSystemSoapClient();
            string strUpdateResponse = ServiceCl.UpdateProductDetails(strProductid, strOfferUnits, strOfferPrice);
            Console.Write("\n---------------------------------------------------------\n"+
                          "\nResponce Received from Service" +
                          "\n---------------------------------------------------------\n" +
                          strUpdateResponse+
                          "\n---------------------------------------------------------\n");
        }
      
    }
}
