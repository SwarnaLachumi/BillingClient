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
    class BillingSystemClient
    {
        private static Service.MarketBillingSystemSoapClient objMarketBillingSystem  = null;
        private static bool isServiceAvailable = false;

        static void Main(string[] args)
        {            
            Console.Write(    "Billing System " +
                             "\n************************************************************************" 
                              );            
            try
            {
                objMarketBillingSystem = new Service.MarketBillingSystemSoapClient();
                isServiceAvailable = objMarketBillingSystem.IsServiceAvailable();
                if (isServiceAvailable)
                {
                    BillingMainMenu();
                }
            }
            catch (Exception e)
            {
                if (!(isServiceAvailable))
                {
                    ErrorNoService();
                }
                else
                {
                    Console.WriteLine("\nError - " + e.Message);
                }
            };

       

        }
        private static void BillingMainMenu()
        {
            Console.Write(  "\nMain Menu" +
                            "\n-----------------------------------------------------------------------------------------------------------------" +
                            "\n=> Type '1' - To view Product Catalogue" +
                            "\n=> Type '2' - To checkout Customer Basket and generate Receipt" +
                            "\n=> Type '3' - To update special offer for Products" +
                            "\n=> Type 'q' - To Quit the application" +
                            "\nEnter:"
                            );
            char chUserInput = Console.ReadKey().KeyChar;
            Console.Write("\n**************************************************************************");
            ReadinputMain(chUserInput);
        }
        private static void ReadinputMain(char chUserInput)
        {
            switch (chUserInput)
            {
                case '1':
                    Console.Write("\nYou have selected Option 1 - Product Catalogue" +
                                  "\n-----------------------------------------------------------------------------------------------------------------");
                    ProductsCatalogue();
                    BillingMainMenu();
                    break;
                case '2':
                    Console.Write("\nYou have selected Option 2 - Checkout Customer Basket and generate Receipt" +
                                  "\n-----------------------------------------------------------------------------------------------------------------" +
                                  "\nPlease enter the Product ID with No. of units purchased in below format \nEg:A99-10,B15-2,C20-1" +
                                  "\nIMPORTANT: Provide input in appropriate format to add product for checkout else it will be ignored" +
                                  "\n-----------------------------------------------------------------------------------------------------------------" +
                                  "\n(Type 'x' <= Back to Main Menu)\n=>Enter your Input:");
                    string strCheckoutlst = Console.ReadLine();

                    if (strCheckoutlst=="x")
                    {
                        BillingMainMenu();
                        break;
                    }
                   
                    CartCheckout(strCheckoutlst);
                    BillingMainMenu();
                    break;
                case '3':
                    Console.Write("\nYou have selected Option 3 - Update Offer Details" +
                                  "\n-----------------------------------------------------------------------------------------------------------------" +
                                  "\n(Type 'x' <= Back to Main Menu)\n=>Enter Product ID to update:");
                    string strProductid = Console.ReadLine();
                    if (strProductid == "x")
                    {
                        BillingMainMenu();
                        break;
                    }
                        Console.Write("\n(Type 'x' <= Back to Main Menu)\n=>Enter No of Units in Offer:");
                    string strOfferUnits = Console.ReadLine();
                    if (strOfferUnits == "x")
                    {
                        BillingMainMenu();
                        break;
                    }
                    Console.Write("\n(Type 'x' <= Back to Main Menu)\n=>Enter Offer Price:");
                    string strOfferPrice = Console.ReadLine();
                    if (strOfferPrice == "x")
                    {
                        BillingMainMenu();
                        break;
                    }
                    UpdateOfferPrice(strProductid, strOfferUnits, strOfferPrice);
                    BillingMainMenu();
                    break;
                case 'q':
                    Environment.Exit(0);
                    break;
                default:
                    Console.Write("\nError: Invalid Input Option");
                    BillingMainMenu();
                    break;
            }
        }
      
        private static void ProductsCatalogue()
        {                       
            try
            {
                DataTable ProductList = null;

                isServiceAvailable = objMarketBillingSystem.IsServiceAvailable();

                if (isServiceAvailable)
                {
                    ProductList = objMarketBillingSystem.ListProducts();

                    ProductList.TableName = "Product_Details";
                    ProductList.Columns.Add("Item_Id");
                    ProductList.Columns.Add("Description");
                    ProductList.Columns.Add("Unit_Price");
                    ProductList.Columns.Add("Sp_Offer_Count");
                    ProductList.Columns.Add("Sp_Offer_Price");

                    Console.Write("\nProduct Catalogue" +
                                  "\n-----------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine("\nSl.No\t\tProduct Id\tDescription\tUnitPrice\tSpecial Offer");
                    Console.WriteLine("\n****************************************************************************************\n");

                    foreach (DataRow dataRow in ProductList.Rows)
                    {
                        string row = String.Join("\t\t", dataRow.ItemArray); ;
                        Console.WriteLine(row);
                    }
                    Console.WriteLine("\n");
                }
              }
            catch (Exception e)
            {
                if (!(isServiceAvailable))
                {
                    ErrorNoService();
                }
                else
                {
                    Console.WriteLine("\nError - " + e.Message);
                }
            }
            
        }
        
      
        private static void CartCheckout(string strItemsList)
        {
            try
            {
                isServiceAvailable = objMarketBillingSystem.IsServiceAvailable();

                if (isServiceAvailable)
                {
                    DataTable Receipt = objMarketBillingSystem.GetReceipt(strItemsList);
                    string GrandTotal = objMarketBillingSystem.GetCostGrandTotal(strItemsList);
                    string[] ValidItemList = objMarketBillingSystem.GetListValidItems(strItemsList);
                    string[] InvalidItemsList = objMarketBillingSystem.GetListInvalidItems(strItemsList);


                    string ValidItemsJoin = string.Join(",", ValidItemList);
                    string InvalidItemsListJoin = string.Join(",", InvalidItemsList);
                    Console.Write("\n-----------------------------------------------------------------------------------------------------------------" +
                                   "\nValid Products:" + ValidItemsJoin +
                                   "\nInvalid Products:" + InvalidItemsListJoin +
                                   "\n-----------------------------------------------------------------------------------------------------------------");


                    Console.Write("\nCustomer Receipt" +
                                 "\n-----------------------------------------------------------------------------------------------------------------" +
                                  "\nSlNo\tItemDescription\t\tUnitPrice\tSpecialOffer\t\tUnits\t\tItemCost" +
                                  "\n***************************************************************************************************\n");
                    foreach (DataRow dataRow in Receipt.Rows)
                    {
                        string row = String.Join("\t\t", dataRow.ItemArray); ;
                        Console.WriteLine(row);
                    }

                    Console.WriteLine("\n-----------------------------------------------------------------------------------------------------------------" +
                                       "\n\t\t\t\t\t\t\t\t\tTotal Amount = " + GrandTotal + "\n");
                }
            }
            catch (Exception e)
            {
                if (!(isServiceAvailable))
                {
                    ErrorNoService();
                }
                else
                {
                    Console.WriteLine("\nError - " + e.Message);
                }
            }

        }
        private static void UpdateOfferPrice(string strProductid, string strOfferUnits, string strOfferPrice)
        {
            try
            {
                isServiceAvailable = objMarketBillingSystem.IsServiceAvailable();

                if (isServiceAvailable)
                {
                    string strUpdateResponse = objMarketBillingSystem.UpdateProductDetails(strProductid, strOfferUnits, strOfferPrice);
                    Console.Write("\n-----------------------------------------------------------------------------------------------------------------" +
                                  "\nResponse Received from Service" +
                                  "\n-------------------------------------------------------------------------\n" +
                                  strUpdateResponse +
                                  "\n-------------------------------------------------------------------------\n");
                }
            }
            catch (Exception e)
            {
                if (!(isServiceAvailable))
                {
                    ErrorNoService();
                }
                else
                {
                    Console.WriteLine("\nError - " + e.Message);
                }
            }
        }

        private static void ErrorNoService()
        {
            Console.WriteLine("\n-----------------------------------------------------------------------------------------------------------------" +
                               "\nError: Sorry the service is not running currently. Please try Later" +
                               "\n-----------------------------------------------------------------------------------------------------------------");
            Console.ReadKey();
        }
      
    }
}
