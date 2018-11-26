const Discord = require('discord.js');
const client = new Discord.Client();
const fs = require('fs');
var settings = require('./settings.js');
var commandSystem = require('./commands.js');

class CommandError extends Error {
    constructor(message,cs,ms){
      super(message)
      this.name = "CommandError"
      this.stack = this.stack + `current cSys:\n${JSON.stringify(cs)}\n message:${ms}\n`;
    }
}

client.on('ready', () => {
  console.log(`Logged in as ${client.user.tag}!`);
});

client.on('message', message  => {
  if (message.content == "foo") {
    let pingOne = Date.now();
    message.channel.send("bar... calculating").then(function(message){
      let pingTime = Date.now() - pingOne;
      message.edit(`b${(pingTime > 2000) ? "aaa... a" : "a".repeat(Math.floor(pingTime/10))}r (${pingTime}ms)`)
    });
  }
  try{  
    if(message.content.toLowerCase().startsWith(settings.prefix)){
      let msg = message.content.substr(settings.prefix.length);
      let cmd = msg.split(/\s+/)[0]
      if(cmd == "reloadsettings"){
        delete require.cache[require.resolve('./settings.js')];
        settings = require('./settings.js');
        console.log(JSON.stringify(settings));
      }else if(cmd == "reloadcommand"){
        delete require.cache[require.resolve('./commands.js')]
        commandSystem = require('./commands.js')
        commandSystem.loadCommands();
        console.log(JSON.stringify(commandSystem.commands));
      }else{
        let cind = commandSystem.commands.findIndex(i => i.name === cmd)  
        if(cind == -1){
          throw new CommandError("The command " + cmd + " doesn't exist", commandSystem, message.content);
        }else{
          commandSystem.commands[cind].func(msg,message,client)
        }
      }

    }
  }catch(err){
    console.error(err)
    let embed = new Discord.RichEmbed()
    .setColor(0xED4D30)
    .setTitle("<:blobexplosion:516363170072231936> Error!")
    .setDescription(err);
    message.channel.send(embed);
  }
});

fs.readFile('token.txt', 'utf8', function(err, data) {  
  if (err){
    if(err.code == "ENOENT"){//no file error
      throw new Error(err.message + " The file \"token.txt\" doesn\'t seem to exist. It should contain be in the same directory as bot.js and contain the bot token.")
    }else{
      throw err;
    }
  } 
  commandSystem.loadCommands();
  console.log(JSON.stringify(commandSystem.commands));
  console.log("Done reading token.txt");
  client.login(data);
});
