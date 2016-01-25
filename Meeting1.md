# 7/12/2007 #

2:30 PM Oge: Hello Chris
> me: hi
> > so i saw your comments/emails
2:31 PM i think the plugin you found for eclipse proves that you can get svg on eclipse
> > but it perhaps at a higher level than we need
> > I ran across this
> > http://xmlgraphics.apache.org/batik/
> > It's a java framework for svg stuff
2:32 PM seems the most mature project out there

> Oge: That's what the eclipse plugin I mentioned uses
> me: ok
> > the eclipse plug-in seems more like an editor
> > they have an example for showing a svg and interacting with it in swing
2:33 PM http://xmlgraphics.apache.org/batik/using/swing.html
> > i'm not sure how you 'paint' things on a eclipse pane
2:34 PM but i would imagine a similiar process. doesn't the plugin support being able to retarget applications for displaying, interacting with svg?
2:35 PM Oge: I'm not sure. Let me check the SVG spec, but AFAIK SVG is just like HTML i.e. sandboxed

> me: well its all about the rendering
> > svg is just a serialized set of graphic calls
2:36 PM with each 'object' can be associated with DOM like interaciton events
2:37 PM wiki has a simple overview
> > http://en.wikipedia.org/wiki/Scalable_Vector_Graphics

> Oge: ok
2:38 PM me: basically
> > the main thing is that SVG support for java is very mature
> > meaning that you can easily get it in eclipse
> > firefox and IE also work with svg pretty well
> > meaning things like TRAC and CruiseControl should work fine with it
> > even python has some decent bindings
2:39 PM however, C# support is actually lacking ;p

> Oge: Hmm, did you see the note I put up about the sample SVG viewer?
2:40 PM me: i think that was mainly hosting an IE webbrowser in a form
> > to render the svg

> Oge: oh...
> me: which works..., but not ideal
2:41 PM Oge: But I think that is what we're going to be doing anyway since at some point we'll want to make call (over TCP perhaps?) to the smell view generator
2:42 PM me: seperate concerns
> > network connection to get data vs display
> > that means something being run without IE would crap out
> > versus tcp communication is still there to get data
2:43 PM Oge: I don't understand...if we did it using TCP to connect viewer and generator, all you'd need is a browser, which can be embedded in Eclipse or VStudio
2:44 PM Could you explain your concern again?

> me: there are two concerns
> > one is communicating across modules
> > the other concern is the rendering module itself
2:45 PM the choice of using a browser as a rendering module has benefits and disadvtanges
> > one disadvantage is that you're communicating across process boundaries between the browser and eclipse
2:46 PM you have to have a host control
> > that subscribes to DOM events within the browser's rendered page
2:47 PM i've done this before, and it works, but it can add complications and dependency on the browser technology

> Oge: true
> > Good point

> me: my overall assessment, is that svg stuff looks promosing
2:48 PM Oge: Is it worth it to implement SVG in .NET?
> me: i think we should have the 'main' tool use svg
> > and get it 'working' in .NET even if not ideal
> > at least I already have a visualization library in .NET that works for now
2:49 PM its just that I didn't want to rewrite it in java, python, etc
> > that's why I think a SVG solution is good
> > and eventually have the .NET use that too

> Oge: Yeah, I agree too
2:50 PM I was just wondering if it was worth it to implement the SVG library, or maybe to look for some other rendering technology
> me: there is stuff out there that does render it
> > its just that its not mature and not active projects

> Oge: ah
2:51 PM me: there is actually a nice vector graphics library for .net but it costs $$$
> > look at VG.net
> > beautiful UIs
2:52 PM Oge: (Its taking a while to load)
> > Perhaps we should list all our options now and then decide which will be most beneficial

> me: that is the name of library
> > let me find site
> > http://www.vgdotnet.com/
2:53 PM Oge: oooh ... pretty :-)
2:54 PM me: essentially the choice is between having to have a seperate rendering engine+visualization library for each environment, or having something that can display the views in a serialized, uniform manner

> Oge: Oh no, I meant, What are our choices as far as output format
> > e.g. SVG, BMP, Flash...

> me: BMP doesn't have interaction
> > or rescaling
2:55 PM Flash is SVG
> > except more heavy weight
> > i've looked into embedding flash into .NET before
> > about the same issues as SVG
2:56 PM just feel like svg has less baggage than flash
> > and it is easier to develop
> > you have to use the macromedia studio
> > there are some tools to generate flash movies
> > but they are just hacks

> Oge: true
> > And then HTML+Javascript?
2:57 PM it is interactive
> > And you don't necessarily have to script the DOM
> > Just refresh after a request is sent

> me: well, for the webpages
> > you'll probably have to end up with SVG+Javascript
2:58 PM but you're basically doing the same thing
> > embedding a browser
> > which at that point, you could display svg anyways
3:00 PM Oge: the only advantage of HTML+js over SV+js is that we don't have to implement any SVG libraries since the plugins to display browsers are (hopefully) already there

> me: there are two things
> > there is the description of the graphics
> > and the rendering engines
> > svg has two rendering engines: native, and browser
> > html has broswer
> > native is nicer if you can do it
3:01 PM but browser is still always there
> > svg has advantage of native support, plus more suitable description

> Oge: But browser support (according to Wikipedia) is not complete in any browser
> > So we'd have to make sure it has all the features we need

> me: 'complete' to the crazy level of specification
3:02 PM we only need a small subset
> > just like no browser supports css ;p

> Oge: :-) true true
3:03 PM me: i think the order of trying is
> > native svg

> Oge: Can you think of any other rendering options? I just want to make sure we check everything else before we commit to this
> me: if not, browser svg
> > if not, html/bmp
3:04 PM i've havent' seen anything in the class of vector graphics in the context of C#
> > basically flash, and microsoft's appollo
> > are the alternatives
3:05 PM Oge: what is appollo?

> me: apollo
> > i think that's what the name is, it's basically a 'adobe killer'
3:06 PM which will not win ;p

> Oge: Hmm...yeah, so I guess we're ruling out Flex then, huh? ;-)
3:07 PM me: flex, as in lex?
> > oops,
3:08 PM apollo+flex are in the same boat
> > i don't know the microsoft name
> > though I know they have lightspeed

> Oge: silverlight or some crap like that
> me: ya silverlight
3:09 PM and google gears is the other end
3:10 PM Oge: All seem sketch. We can probably stick comfortablywith SVG for now
> > So that's one issue down

> me: ya, i think flex, etc is something interesting to check out in the future
3:11 PM Oge: Now, how about the communication b/w generator and renderer?
> > TCp/IP?
3:12 PM me: well, its more between the 'invoker' and generator

> Oge: yeah
> me: having to have something to run as a service is kinda of an adminstrative pain
3:13 PM Oge: I know
> me: i think it should be configured
> > to either allow command line
> > or through service
> > the real communication is the data
> > which would be xml, or whatever

> Oge: How do you mean through command line?
3:14 PM me: like evocating a shell script,etc
> > system.Run("cmd")

> Oge: Sort of like CGI, eh?
> me: ya
> Oge: that's a good idea
> me: that's the easiest way to get it up and running
3:15 PM in this case its more about the data, than initiated and managing the communication
3:16 PM the invoker basically needs to give the generator the location of the code
> > perhaps, a serialized list of the source files, directories
> > the generator does it thing
> > and the invoker just reads in the generated images
3:17 PM Oge: yeah
> > So we're pretty much done as far as architecture

> me: ya, that's the initial stab
3:18 PM basically, the test would be
> > to see if you could get eclipse to give you the list of project files
> > send that to some 'script
> > that then tells you which methods have long methods
> > there is the 'svg' generation
3:19 PM that is a major module to be developed
> > but if you wanted to just test the workflow, you use a test .svg image
> > and then the invoker loads that svg back in, with the 'site' description that allows you to click on the image, and navigate you to the place in code

> Oge: Don't forget we still have to write Eclipse and VS SVG renderers. That will be fun
3:20 PM me: well eclipse as the batik tool kit
> > which should done most of the work
> > has8
3:21 PM Oge: What I meant was the little something we'll add to translate SVG events into IDE events
> > But we'll cross that soon enough

> me: the click events?
> Oge: Yeah, for instance
> me: ya, there has to be code written to navigate to a location in code
3:22 PM i already have that for Visual studio
> > i'm sure it should be easy in eclipse too
3:23 PM Oge: okey-dokey
> > So, what's the next steps
> > We should get an agenda for our next meeting

> me: i think basically have a proof of concept
3:24 PM Oge: That shouldn't be tooooo hard. At least for the generator side
3:25 PM me: ya, i think having the generator just be dumb and return a dummy image is good enough
> > for proof of concept

> Oge: Ah, I think here is where we have to decide on a language(s), decide on modules and then see what we can start with
3:26 PM me: well, i think the best thing for creating svg images is with the batik toolkit
> > though i think python has some ok support for it too
3:27 PM Oge: sweet!!!

> me: so that can be done in java
> > the code scanning might be easier in python ;p
3:28 PM Oge: Alright. So for action points:
3:29 PM 1. Explore other SVG librarries then decide on which languagee/lib to use
> > 2. Make a "generator" backend that spits out a simple SVG image
> > izzat it?

> me: well, the 3
> > is make that image show in eclipse
3:30 PM and 4 is click on image in eclipse, and make it go to a code location

> Oge: whooo. We get that done by Thursday, all drinks are on me :-)
> me: ha, ok
3:31 PM Oge: Alrighty. I'll send in my notes on which language/lib I'd prefer to use by tomorrow. We can compare notes and then begin work
> me: i'm putting this stuff in the google.code
3:32 PM Oge: Okey-dokey. Could you also add our meeting logs? Its nice to be able to search them all in one place
> me: i guess at the wiki
> Oge: sure, yeah
> me: k, sign out
3:33 PM rather, meeting adjourned
> Oge: ciao ciao