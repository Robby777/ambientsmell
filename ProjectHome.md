# Introduction #
Code smells indicate possible design problems.  Cleaning up bad smells is a continual battle.  Ambient Smell alerts developers of possible bad smells through light weight visualizations and allow for a quick way of performing a code review.

# Meet The Smells #

## Long Parameter ##
![http://www.ambientsoftvis.com/images/smells/longparamex.png](http://www.ambientsoftvis.com/images/smells/longparamex.png)

## Message Chain ##
![http://www.ambientsoftvis.com/images/smells/messageChain.png](http://www.ambientsoftvis.com/images/smells/messageChain.png)

## More... ##
[MeetTheSmells](MeetTheSmells.md)

# Target Systems #
Ambient Smell is intended to run on the following systems:

  * CruiseControl
  * TRAC
  * Eclipse
  * Visual Studio 2003
  * Visual Studio 2005
  * Ambient Office

# Architecture #

The system is composed of two main modules.  A code smell scanner, and a code smell viewer.

## View Generator ##
The view generator renders bad smell candidates visualized in a svg file and information on location and metrics values in a xml file.

Environment =(Code Info)=> Scanner =(Smell Candidates)=> View Generator => (Visualization, SmellMetaInfo)

## Viewer ##
The viewer displays the svg file and responds to click events to display the appropriate location in the environment.