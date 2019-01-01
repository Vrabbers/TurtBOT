const Discord = require('discord.js');
var command = [];  
const {reloadCommands, reloadSettings} = require("./bot.js");

function makeCommand(acname, acfunc){
    command[command.length] = {name: acname, func: acfunc}
}
function loadCommandH(){
    makeCommand('foo', (msgstr,message,client)=>{
        let pingOne = Date.now();
        message.channel.send("bar... calculating").then(function(message){
          let pingTime = Date.now() - pingOne;
          message.edit(`b${(pingTime > 2000) ? "aaa... a" : "a".repeat(Math.floor(pingTime/10))}r (${pingTime}ms)`)
        });
    });
    makeCommand('help',(msgstr,message,client)=>{
        if(msgstr.toLowerCase().substr(5).length == 0){     
            let commandList = ""
            for(i in command){
                commandList += command[i].name + "\n"
            }
            let embed = new Discord.RichEmbed()
            .setTitle("Help")
            .setDescription(commandList)
            .setFooter("Command-specific help is not yet available!")
            .setColor(0x0AF50A);
            message.channel.send(embed);
        }else{
            throw "Command-specific help is not a thing yet!"
        }
    });
    makeCommand('exception',(msgstr,message,client)=>{
        throw "fucko boingo";
    })
}
module.exports = {
    loadCommands: loadCommandH,
    commands: command
}
