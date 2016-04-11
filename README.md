# GoogBot-Recode

This was an attempt to recode my GoogBot/Mitzi IRC bot. I wanted to use a new IRC library, since I thought the one I was using at the time was bad (it wasn't it was my own coding, and IRCSharp is complete crap). I had also just discovered Dictionaries and Func/Action in C#, which made this one *slightly* better than the original. Except that to create a new command it required changing lines in like 4 different places. It had module support though, which could be enabled/disabled and saved, which was kinda cool. This was intended to be a public bot that people could host/modify themselves, and I even spent a day making a config generator that would ask a series of questions in console on first launch.

After a while of fighting with how horrible IRCSharp was (it would hang after being kicked from a channel then attempting to rejoin), I gave up on it, and shortly afterwards found Discord. I used this for prototyping RoboNitori's !playing command for Gensokyo Radio.

I can't remember if the config manager still works, but if it does, you could probably get this running pretty easily.
