const Discord = require('discord.js');
var command = [];  

function makeCommand(acname, acfunc){
    command[command.length] = {name: acname, func: acfunc}
}
function loadCommandH(){
    makeCommand('foo', function(msg,message,client){
        let pingOne = Date.now();
        message.channel.send("bar... calculating").then(function(message){
          let pingTime = Date.now() - pingOne;
          message.edit(`b${(pingTime > 2000) ? "aaa... a" : "a".repeat(Math.floor(pingTime/10))}r (${pingTime}ms)`)
        });
    });
    makeCommand('help',function(msg,message,client){message.channel.send(command.join("\n"))})
}
module.exports = {
    loadCommands: loadCommandH,
    commands: command
}
