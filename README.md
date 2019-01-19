# BillingClient
Billing Client Application acts as Supermarket checkout basket tool for calculating TotalPrice from Web Service.

Requirements:
Client must consume the service to calculate the total price for a number of items by summing their prices including applying any relevant discounts. Weekly offers for products change frequently so it is important to provide the ability to change them.

It has three Main options Menu 1 - To visit Products Catalogue | Menu 2 - To checkout Customer Basket and generate Receipt with Total Amount applying discounts | Menu 3- To Update Offer Prices to Products

Below details states the input and output pattern for each menu


Menu 1 - To view Product Catalogue | Input:NA | Output: Returns all the Products  from the DB in catalogue View

Menu 2 - To checkout Customer Basket and generate Receipt with Total Amount applying discounts |
        Input: SKUProduct<1>ID-No.ofUnits,SKUProduct2ID-No.ofUnits,SKUProduct<3>ID-No.ofUnits........SKUProduct<n>ID-No.ofUnits |
        Eg: A99-10,B15-2,C20-1 | | Each item should be demilited by comma ',' | Output: Returns Receipt with Total  from web service
         
Menu 3- To Update Offer Prices to Products | Input1: SKUID of Product for which offer to be updated | Input2: Number of Units comes in Special offer | Input3: Price for the offered Units | Output: Returns the status of update from web service
        
  
