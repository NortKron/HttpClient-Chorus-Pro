# HttpClient-Chorus-Pro
Http-Client prototype for submitting invoices to the portal [Chorus Pro](https://portail.chorus-pro.gouv.fr/aife_csm/?id=aife_index)

The console program does the following steps
1. Receives a access token
2. Send invoice in XML format. Receives a request number in flow.
3. Requests status every 30 seconds until it is received
4. Displays information about invoice status and errors
