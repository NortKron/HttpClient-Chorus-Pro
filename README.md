# HttpClient-Chorus-Pro
Http-Client prototype for submitting invoices to the portal [Chorus Pro](https://portail.chorus-pro.gouv.fr/aife_csm/?id=aife_index)

The console program does the following steps
1. Receives a access token
2. Send invoice in XML format. Receives a request number in flow.
3. Requests status every 30 seconds until it is received
4. Displays information about invoice status and errors

Example log of the program
<p align="center">
  <img src="https://user-images.githubusercontent.com/6648495/172368333-3a86b285-7dad-48c3-90f8-7fcc4eaec534.png" width="640">
</p>
