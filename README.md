# G20-XMLDB-Server
A simple MySql based Mabinogi database server, and authenticator server.

# Usage:
Install MySql
Run XMLDB.exe once to generate the Json config files

Add an administrative MySql user to the Default Connection field in Config.json, and run the executable again to generate the databases and tables.

Close the server by typing `shutdown` twice (I have yet to consolidate the console reading for both servers), change the XMLDB servers `config.json` `First run: true` to `false`, and add your database values to the fields (I am leaving this functionality intact, incase people wish to use different servers or databases), and that is it, set up complete!

Run the server, and continue on the regular steps to running a server.

# Todo:
* Currently there is "no" way to add accounts or cards other than by manually enterting them into the database, this is coming shortly via the DataBridge class.
* Other example functions using the DataBridge! :D

# Note:
There are SQLite functions, but they are not supported, they are from an attempt to implement a locally based server, but SQLite does not function well with the database structure (deletion of items was impossible, somehow keys were circular...), but they remain if anyone wishes to attempt to implement!

Please pull request! :D