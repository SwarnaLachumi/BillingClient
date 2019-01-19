# BillingClient
BillingClient to access Web Service
Type:  Console application (Windows Client)
Name: BillingClient
Database: NA

C# simple client and service for a supermarket checkout basket total.
Menus:
Menu 1 - To visit Products Catalogue
        Input:NA

Requirement - The service must calculate the total price for a number of items by summing their prices
including applying any relevant discounts.        
Menu 2 - To checkout Customer Basket and generate Receipt with Total Amount applying discounts
        Input: SKUProduct<1>ID-No.ofUnits,SKUProduct2ID-No.ofUnits,SKUProduct<3>ID-No.ofUnits........SKUProduct<n>ID-No.ofUnits
        Eg: A99-10,B15-2,C20-1
        Each item should be demilited by comma ','
        
Requirement - Weekly offers change frequently so it is important to provide the ability to change them.
Menu 3- To Update Offer Prices to Products
•	Input1: SKUID of Product for which offer to be updated
•	Input2: Number of Units comes in Special offer
•	Input3: Price for the offered Units

