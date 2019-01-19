using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using BillingClient.Service;


namespace BillingClient
{
    class Program
    {

        private static Service.MarketBillingSystemSoapClient ServiceCl = new Service.MarketBillingSystemSoapClient();
        
        static void Main(string[] args)
        {
            Console.Write(    "Billing System " +
                             "\n************************************************************************" 
                              );
            try
            {
                DataTable test = ServiceCl.ListProducts();
                StandardMainTemplate();
                char input = Console.ReadKey().KeyChar;
                ReadinputMain(input);

            }
            catch {
                ErrorNoService();
            };
          
        }
        private static void StandardMainTemplate()
        {
            Console.Write(  "\nMain Menu" +
                            "\n-------------------------------------------------------------------------" +
                            "\n=> Type '1' - To view Product Catalogue" +
                            "\n=> Type '2' - To checkout Customer Basket and generate Receipt" +
                            "\n=> Type '3' - To update special offer for Products" +
                            "\nEnter :"
                            );
            char input = Console.ReadKey().KeyChar;
            Console.Write("\n**************************************************************************");
            ReadinputMain(input);
        }
        private static void ReadinputMain(char input)
        {
            switch (input)
            {
                case '1':
                    Console.Write("\nYou have selected Option 1 - Product Catalogue" +
                                  "\n-------------------------------------------------------------------------");
                    listProductList();
                    StandardMainTemplate();
                    break;
                case '2':
                    Console.Write("\nYou have selected Option 2 - Checkout Customer Basket and generate Receipt" +
                                  "\n-------------------------------------------------------------------------" +
                                  "\nPlease enter the Product ID with No. of units purchased in below format \nEg:A99-10,B15-2,C20-1" +
                                  "\nIMPORTANT: Provide input in appropriate format to add product for checkout else it will be ignored" +
                                  "\n-------------------------------------------------------------------------" +
                                  "\n(x <- back to Main Menu)=>Enter your Input:");
                    string strCheckoutlst = Console.ReadLine();

                    if (strCheckoutlst=="x")
                    {
                        StandardMainTemplate();
                        break;
                    }
                   
                    CheckOut(strCheckoutlst);
                    StandardMainTemplate();
                    break;
                case '3':
                    Console.Write("\nYou have selected Option 3 - Update Offer Details" +
                                  "\n-------------------------------------------------------------------------" +
                                  "\n( x <- back to Main Menu)=>Enter Product ID to update:");
                    string strProductid = Console.ReadLine();
                    if (strProductid == "x")
                    {
                        StandardMainTemplate();
                        break;
                    }
                        Console.Write("\n( x <- back to Main Menu)=>Enter No of Units in Offer:");
                    string strOfferUnits = Console.ReadLine();
                    if (strOfferUnits == "x")
                    {
                        StandardMainTemplate();
                        break;
                    }
                    Console.Write("\n( x <- back to Main Menu)=>Enter Offer Price:");
                    string strOfferPrice = Console.ReadLine();
                    if (strOfferPrice == "x")
                    {
                        StandardMainTemplate();
                        break;
                    }
                    UpdateOfferPrice(strProductid, strOfferUnits, strOfferPrice);
                    StandardMainTemplate();
                    break;
                default:
                    Console.Write("Error: Invalid Input Option");
                    StandardMainTemplate();
                    break;
            }
        }
      
        private static void listProductList()
        {
            DataTable ProductList = ServiceCl.ListProducts();
            ProductList.TableName = "Product_Details";
            ProductList.Columns.Add("Item_Id");
            ProductList.Columns.Add("Description");
            ProductList.Columns.Add("Unit_Price");
            ProductList.Columns.Add("Sp_Offer_Count");
            ProductList.Columns.Add("Sp_Offer_Price");
            Console.Write("\nProduct Catalogue" +
                          "\n-------------------------------------------------------------------------");
            Console.WriteLine("\nSl.No\t\tProduct Id\tDescription\tUnitPrice\tSpecial Offer");
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
            DataTable Receipt = ServiceCl.GetReceipt(strItemsList);
            string GrandTotal = ServiceCl.GetCostGrandTotal(strItemsList);
            string[] ValidItemList = ServiceCl.GetListValidItems(strItemsList);
            string[] InvalidItemsList = ServiceCl.GetListInvalidItems(strItemsList);

            string ValidItemsJoin = string.Join(",", ValidItemList);
            string InvalidItemsListJoin = string.Join(",", InvalidItemsList);
            Console.Write("\n-------------------------------------------------------------------------" +
                           "\nValid Products:" + ValidItemsJoin +
                           "\nInvalid Products:" + InvalidItemsListJoin+
                           "\n-------------------------------------------------------------------------");


            Console.Write("\nCustomer Receipt" +
                         "\n-------------------------------------------------------------------------" +
                          "\nSlNo\tItemDescription\t\tUnitPrice\tSpecialOffer\t\tUnits\t\tItem_Cost"+
                          "\n***************************************************************************************************\n");
            foreach (DataRow dataRow in Receipt.Rows)
            {
                string row = String.Join("\t\t", dataRow.ItemArray); ;
                Console.WriteLine(row);
            }

            Console.WriteLine("\n-------------------------------------------------------------------------" +
                               "\t\t\t\t\t\t\t\t\t\t\t\tTotal Amount = " + GrandTotal +"\n");
            
        }
        private static void UpdateOfferPrice(string strProductid, string strOfferUnits, string strOfferPrice)
        {
            string strUpdateResponse = ServiceCl.UpdateProductDetails(strProductid, strOfferUnits, strOfferPrice);
            Console.Write("\n-------------------------------------------------------------------------" +
                          "\nResponse Received from Service" +
                          "\n-------------------------------------------------------------------------\n" +
                          strUpdateResponse +
                          "\n-------------------------------------------------------------------------\n");
        }

        private static void ErrorNoService()
        {
            Console.WriteLine("\n-------------------------------------------------------------------------" +
                               "\nError: Sorry the service is not running currently. Please try Later" +
                               "\n-------------------------------------------------------------------------");
            Console.ReadKey();
        }
      
    }
}
