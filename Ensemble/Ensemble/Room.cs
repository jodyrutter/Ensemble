using System;
using System.Collections.Generic;
using System.Text;

namespace Ensemble
{
    public class Room
    {
        public List<string> participants { get; set; } //emails of participants
        public MessageContent lastMsg { get; set; } //display last msg along with the time
        public String Name { get; set; } //Display name of room
        public List<MessageContent> ChatLog { get; set; }  //Chat log of room
        public Room()
        { 
        
        }

        public Room(List<string> ppl, MessageContent lastMsg, string name, List<MessageContent> ChatLog)
        {
            participants = ppl;
            this.lastMsg = lastMsg;
            Name = name;
            this.ChatLog = ChatLog;
        }

        public Room(List<string> ppl, string name)
        {
            participants = ppl;
            lastMsg = null;
            Name = name;
            ChatLog = null;
        }
    }
}
