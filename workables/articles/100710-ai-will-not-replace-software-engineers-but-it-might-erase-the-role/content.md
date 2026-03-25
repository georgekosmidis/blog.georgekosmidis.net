Every few weeks a new headline lands: "AI will replace software engineers." Then someone writes the counter-argument: "No it won't, engineers do more than write code." Both sides are wrong.

AI will not replace software engineers - but it will shrink the role until it transforms into something new. The economy will absorb the shock - it always does, and I covered the macro case in detail in [The Historical Parallel, 1987: Computers, 2025: AI](https://blog.georgekosmidis.net/the-historical-parallel-1987-computers-2025-ai.html): **The WEF projects a net gain of 78 million jobs by 2030.**

But here is the part that should keep you up at night: **the economy absorbing the shock is not the same thing as *you* absorbing the shock**. The PC revolution was net positive too. It still destroyed the career of nearly every typist, switchboard operator, and filing clerk who could not make the jump to whatever came next. Those people did not experience a "net positive outcome". They experienced a profession that transformed into something they no longer qualified for.

This article is about why software engineering is next in line for that kind of transformation - and why the window to prepare is shorter than most of us think.

## The Middle Layer

To understand why software engineering is vulnerable, strip the job down to its **usual** structural function. Remove the frameworks, the tooling debates, the job titles. What the most of the software engineers do is this: **build and maintain the link between an interface and a persistence layer**. A user or machine does something, logic applies, data transform and move, state changes, something gets stored or retrieved. That work can be enormously complex - ingestion pipelines, distributed systems, intricate UIs, deep cloud expertise, tens of engineers working for months - or it can be straightforward CRUD operations. But at every level of complexity, the core function is the same. Everything else - the design patterns, the architecture decisions, the test coverage, the deployment pipelines, even the UI - exists to make that link work reliably and at scale.

> This does not describe every corner of the profession - kernel developers or embedded engineers operate differently. But it describes the work that the majority of software engineers do, and the work most exposed to what comes next.

We have built entire careers, communities, and identities around this middle layer but that is the part most directly threatened by agentic AI and this is not a hypothetical. **Entry-level employment for software developers aged 22-25 has already declined nearly 20% from its 2022 peak**. Junior job postings have dropped roughly 29% since January 2024. The middle layer is already contracting - the question is why, and how far it goes.

## Why Agentic AI Changes the Equation

The threat is not that AI writes code faster. **The threat is that the code may not need to exist in the first place**.

When AI agents can connect directly to services, databases, and APIs - through standardized protocols like MCP or whatever succeeds it - the need for a human to hand-build the link between interfaces and data shrinks. Today, if you want an app that shows your calendar, your emails, and your task list in one view, an engineer needs to wire up three different APIs, handle authentication, manage state, deal with errors, and build the UI. In the agentic world, an agent connects to three services through a protocol layer. The glue code that an engineer would write becomes a configuration problem that a machine can solve.

This is not science fiction. MCP has already been adopted by OpenAI, Microsoft, Google DeepMind, and thousands of developers. The protocol is still maturing - *its [2026 roadmap](https://thenewstack.io/model-context-protocol-roadmap-2026/) is focused on basic reliability and security problems that are far from solved* - but the direction is clear. And MCP is just one example of a broader trend: the standardization of machine-to-machine communication that bypasses the human engineer in the middle.

## The Travel Agent Precedent - Jevons Paradox

Before going further, there is a popular counterargument that deserves a fair hearing. **Jevons Paradox** says that when you make a resource cheaper to use, people do not use less of it - they use more. Coal got more efficient, coal consumption exploded. Computing got cheaper, we put computers in everything. So if AI makes software cheaper to build, we will just build vastly more software and need even more engineers to do it.

But Jevons Paradox has a critical assumption: the technology must be **complementary** to the worker. A better framework still needed someone to use it. A cloud platform still needed someone to configure it. The efficiency made human engineers more valuable, not less necessary, because the human was always still in the loop.

The history of travel agents shows what happens when the technology becomes **substitutive** instead.

In the 1990s, the internet began making it possible for consumers to book flights, hotels, and rental cars directly. Demand for travel did not shrink - it exploded. Air travel alone roughly doubled between 1990 and 2019. Jevons Paradox worked exactly as predicted on the demand side. People wanted far more travel than ever before.

But travel agent employment in the U.S. fell by more than half between 2000 and 2022, from roughly 140,000 to around 64,000 according to BLS occupational data - even as air travel nearly doubled over the same period. More travel, fewer travel agents - because the internet did not make travel agents more productive. It replaced the core function they performed: the middle layer between a customer who wanted to go somewhere and the airlines, hotels, and car rental companies that could take them there.

## A Role Transformation for Macroeconomics

The travel agent analogy is useful, but it is also incomplete in an important way. Travel agents did not go to zero. They contracted sharply and then **transformed**. The ones who survived stopped booking simple flights and started specializing in complex itineraries, corporate travel management, luxury experiences, and group logistics - work that required judgment, relationships, and domain expertise that a booking website could not replicate.

The same pattern played out across the board. Bank tellers became financial advisors. Typists became data analysts. The role transformed into something adjacent but fundamentally different, requiring capabilities the old role did not demand.

New roles will emerge - the scorecard from the [previous article](https://blog.georgekosmidis.net/the-historical-parallel-1987-computers-2025-ai.html) already names them: AI system architects, human-AI collaboration designers, AI ethics and governance specialists. But those labels describe an economy. They do not describe your career.

## A Role Elimination for Individuals

The transformed roles that emerged from every previous technology shift did not absorb everyone who was displaced. Many simply left the profession entirely because the new role required capabilities they did not have and could not quickly acquire.

In software engineering, the early signs of that split are already visible.

Entry-level employment for software developers aged 22-25 has declined nearly 20% from its 2022 peak - the sharpest drop among the occupations studied (Stanford Digital Economy Study, 2025). Job postings for junior and entry-level positions have fallen roughly 29% since January 2024 (Randstad/WEF). The tasks that companies used to hand to junior developers - writing boilerplate, fixing basic bugs, building simple features - are increasingly handled by AI tools.

The market is not collapsing. It is **splitting**. At the top, senior engineers and AI specialists command strong salaries and growing demand. At the bottom, the entry pipeline is drying up. The middle is getting crowded with experienced engineers competing for roles that used to be abundant. This is the same K-shaped pattern that emerged in the 1990s - and it is consistent with every previous technology transition.

If you are a mid-career engineer who has spent years building expertise in the middle layer, this is the pattern that should concern you most - not because the economy lacks jobs, but because the jobs it creates may not match what you have spent a decade learning.

## The Uncertain Steps

If the PC-era timeline holds - and the data in the [productivity paradox](https://blog.georgekosmidis.net/the-historical-parallel-1987-computers-2025-ai.html) article suggests it does - you have roughly five to eight years before the full reshuffling is complete. That is not a long time. It is roughly the length of a single job tenure.

But there is a version of this story where the transformation stalls - and it is worth taking seriously.

If AI plateaus at code generation and stays weak at architecture, integration, and requirements negotiation, then senior engineers retain a durable role and Jevons Paradox may hold after all. That is a real possibility, not a strawman.

But notice what even the optimistic version of this argument concedes: **the lower steps of the ladder are already being substituted. The debate is about how many steps, not whether any steps at all. And if there are no lower steps left, where do you find the experience to climb?**

Beyond the plateau question, there are genuine obstacles that could slow the timeline. Security is the most obvious. Prompt injection, token theft, tool poisoning - every agentic system today is an attack surface that requires skilled engineers to secure and monitor. As I wrote earlier, MCP's own [2026 roadmap](https://thenewstack.io/model-context-protocol-roadmap-2026/) is focused on these basic reliability and security problems. If those problems prove harder than expected, the transition drags out. Compliance and regulatory friction could do the same: industries like healthcare, finance, and defense will not hand over critical integration logic to an agent without auditability and accountability guarantees that do not yet exist. And there is the trust gap - most organizations are still not comfortable letting an AI agent make unsupervised changes to production systems, for good reason.

Unfortunately though, none of this means the transformation stops. It means the timeline stretches, the middle layer contracts unevenly across industries, and the transition is messier than a clean [five-to-eight year window](https://blog.georgekosmidis.net/the-historical-parallel-1987-computers-2025-ai.html#the-paradox-will-resolve-it-always-does) suggests. But "slower and messier" is not the same as "not happening". The early internet had all of these problems too - security was terrible, regulation was nonexistent, trust was low - and it still reshaped every industry it touched. The question for you, individually, is not whether the obstacles are real. They are. The question is whether you want to bet your career on them being permanent.

## The Honest Part

It would be nicer to write a post celebrating how AI makes engineers more productive and how the future is bright for anyone who learns to prompt well. It would also be dishonest.

Most of us who have been writing code for years do not think of "software engineer" as a job description. It is an identity. That is exactly why this needs to be said clearly - by people who love the craft, not by people dismissing it from the outside.

Some engineers have already lived a small-scale version of this transformation - moving from writing code to designing systems, from building the middle layer to deciding what it should look like. That shift sometimes happened by plan, but it can also happen because the work changed and adaptation followed. But adaptation is not guaranteed, and not everyone will have the same runway.

The PC-era transition teaches two things simultaneously. First, the economy comes out stronger on the other side. Second, the individuals inside the contracting roles often do not. Both are true at the same time, and pretending the first cancels out the second helps nobody.

The question is not whether AI will replace software engineers. The macro data says it will not. The question is whether the role as we know it - the scope, the daily work, **the compensation** - will still exist when the transformation is complete. And whether we will have the skills required by the new roles.

History is clear on who benefits from the reshuffling: the people already in motion when the breakout hits - not the ones who saw it coming and stood still.