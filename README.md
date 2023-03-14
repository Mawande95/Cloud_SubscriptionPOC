# Clud Subscription Project

# About the Project
This is a web based application that allows people/customers to subcribe or buy cloud services.

#Installation
The project is developed in .Net 6, Visual Studio 2022 and MS Database. 
1.First you need to restore the back up database(CloudSubscriptionDB.bak), 
2.Clone/download a solution to your local machine.
3 After you have succesfully downloaded solution, you will notice that CloudSubscription solution has projects, CloudSubscriptionAPI, CloudSubscriptionWeb and Entities.
4 In your CloudSubscriptionAPI project open "appsettings.Development" file and change "DefaultConnection" to link to your database.
5 In your CloudSubscriptionWeb project open  "SD" file and change your "APIBaseURL" url to match  CloudSubscriptionAPI project local url.

#Usage
1.The default page is a Homepage, which diplays a number of cload services.
2.Once you hover any Cloud service, you will notice that each and every particular cloud service offer has it's own attributes.
2.When you are interested in buying one of them, just click "Buy" button in that particular cload services card, then you will be redirected to login page.
3 If you do not have an account then click register link (Please NOTE that you have to remember your login credentials since there's not form of email/sms communication in this system).
4 After registration, the system will go back to Login page,
5 After Login, the system will redirect to Dashbord page, you are going to see the details of the cloud service you have chosen in the Homepage.
6 Then click "Continue to checkout"  button that will redirect you to PayPal page.
