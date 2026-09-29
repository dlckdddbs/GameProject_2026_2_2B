let express = require('express');
let app = express();

app.get('/about' ,function (req,res) {
    res.send('url');
});

app.listen(3000,function()
{
    console.log('listening on port 3000');
});