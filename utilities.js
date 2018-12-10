const Discord = require("discord.js");

function findUserGuild(id, guild){
    if(guild.members.has(id)){
        return guild.members.get(id);
    }else if(typeof guild.members.find("displayName", id) !== "undefined"){
        return guild.members.find("displayName", id);
    }else if(typeof guild.members.find(val => val.user.username === id) !== "undefined"){
        return guild.members.find(val => val.user.username === id);
    }
}
