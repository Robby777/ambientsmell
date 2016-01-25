12:41 PM Oge: Hey, I tried using the Batik SVGCanvas in Eclipse but (as noted in the Issue tracker) their graphical libraries are not compatible.
> I'm not sure what avenues we have left for Eclipse

1:02 PM me: hey
> Oge: Hello
> me: so what graphics model does eclipse use?
> > swt?

> Oge: It uses the SWT library
1:03 PM And Batik uses AWT
> > And apparently, the two don't go together

> me: so until there is a good swt layer for batik it won't work
> Oge: Seems like it
> me: so i think the most logically thing (since .net does have some problems with svg too)
1:04 PM is only do the display in a browser
> > the browser is the universal platform

> Oge: crudola
1:05 PM me: or we can just have a seperate rendering layer for eclipse
1:06 PM Oge: You mean, write our own rendering layer?
> me: not svg
> > but just use swt to display what we need

> Oge: That's an option
1:07 PM I'm wondering whether we could just stick with one platform or whether it would be better to just go with the browser
> me: one option to do is go with cruisecontrol
> > which is more indepentant of the build environment
> > and is basically the browser
1:08 PM basically decide what is the main venue

> Oge: So what do we get from using cruisecontrol that we can't get by simply using a browser
> > ?

> me: cruisecontrol intergrates itself into the build process
1:09 PM it abstracts away the source control system
> > cruisecontrol really is more of an entry point for display a web based report
1:10 PM but i don't think it would be hard to have eclipse version
1:11 PM Oge: Hmm, does it have an in-built code browser? That would allow us to add some interactivity...
> > Re: eclipse. Do you mean, it won't be hard to draw the smells in Eclipse?

> me: ya, if you just go the swt route
> > we already have it working for VS
1:12 PM just abstract out the processing code more
> > and have it draw in Eclispe
1:13 PM and that would be two platforms

> Oge: Ah. So, for example, generate an XML file with all the processed data and then each platform can render as it likes?
> me: ya
1:14 PM if the interface is good, then anyone can develop their own plug-in to display it ;p
> Oge: :-)
> > 

&lt;sniff&gt;

 SVG woulda been cool, though

> me: yes...
> > but the man wants to keep us down

> Oge: hahahahaha
1:15 PM me: i guess, another way to put it is that the processing code is a challenge in itself
1:16 PM Oge: I doubt it. Nothing a good grepping won't fix :-)
> me: i only ever got to do a small subset of the smells
> Oge: oh...
> me: basically, my approach was a simple parser
1:17 PM it was a line-based approach
> > that tokenized the results
> > but smart enough to handle comments, multi-line comments, etc
> > and you could ask a line if it had the presence of certain things
> > like if statement, ;, while loop
1:18 PM Oge: Since Eclipse does a lot of this, we could prob'ly steal a lot of code from there

> me: yes, that is the advantage of having the IDE as a front end for that sort of stuff
> > VS didn't really have that facility for C# at the method level, it did for C++
1:19 PM so having a source code model that had enough information for a processor to run over is good

> Oge: Alrighty. So could you put up a list of the smells on the wiki or in the issues page?
1:20 PM me: hmm ok
> Oge: We could then track what needs to be done
> me: i can do that now
> Oge: sweet
1:23 PM Then, I think the next task we face is to find (or write) a source analysis tool with which we can extract data for the smells
1:24 PM me: well, one question is to what extent does eclipse provide that facility?
> > we have mult-language problem here
> > the more accurate you try to get, the more specific per language you need to be
> > if you go more fuzzy, then you can abstract away some of the differences
1:25 PM Oge: Well, Eclipse seems to be able to do it for scads of languages pretty well. I don't know how much code each plug-in shares though
> > That is a topic of research
> > You wanna assign that Issue to me?
1:27 PM me: for example, there is the python plugin
> > but i don't know how specific to that plug-in environment you have to be
> > or if there is some sort of meta-plug-in you can have that just refers to the active environment ;p
1:28 PM there is also another approach you can use the .NET world
> > which is scan the bytecode
1:29 PM the nice thing about that is the bytecode is very close to the source code, but easier to process because it has already been compiled and has all the type information there
> > however, you do lose things like comments

> Oge: None of the smells concern coments do they?
1:30 PM me: in long methods for example, it is one of the things colored
> > if it has comments
> > but i think that's the exception
> > basically, you'll always fine a much more mature byte-processing framework than a parsing framework
1:31 PM which is usually stand-alone
> > i know that is the approach the mono project is taking for bad smells
> > they are using a byte code scanner for looking for long-smells
> > bad smells
> > its just another design dimension to confuse us with ;p
1:32 PM Oge: Ah, I ain't scared of parsing source code. Bring it on

> me: alright
1:34 PM Oge: I've added an issue to Google Code. if you find any parsing libraries or parts of libraries we can use, please add it to the Issue
1:35 PM i'll probably be able to report on it by Thursday
> me: ok cool
> 46 minutes
2:21 PM me: http://code.google.com/p/ambientsmell/wiki/MeetTheSmells