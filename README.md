PRO Center
=============

PRO Center is an open source software applicatin sponsored by the U.S. Substance Abuse and Mental Health Administration (SAMHSA) which is designed to be a platform for hosting standardized assessments/screeners. PRO Center is an acronym for Patient Reported Outcomes Center which states one of the goals of the system. 

<b>Watch online Demo Video:</b>
<a href='http://obhita.github.io/PROCenter/'>PRO Center Demo</a>

<b>Key features:</b>
 - Patient Portal
 - Standardized Assessments
 - Ability to integrate with Electronic Health Record (EHR)/ Electronic Medical Record (EMR) systems.
 - Reporting/Charting against data in the system.

<b>Software Architecture Design:</b>
<ul>
 <li> Domain Driven Design (DDD): Based on the following building blocks: entities, value objects, aggregates, domain events, factories, repositories, and services. http://dddcommunity.org/learning-ddd/what_is_ddd/</li>
<li>SOLID Principles:
   <ul>
   <li>Single Responsibility Principle - Each Class should have a unique responsibility or feature.</li>
   <li>Open Closed Principle � A class must be open to extensions but closed to modifications.</li>
   <li>Liskov Substitution Principle � Sub-types must be able to be replaced by their base types.</li>
   <li>Interface Segregation Principle � The class interface implementers should not be obliged to implement methods that are not used.</li>
   <li>Dependency Inversion Principle � Abstractions should not depend on details; the details should depend on abstractions.</li>
   </ul>
</li>
<li>DDD Application Layers: Per DDD principles, PRO Center is designed with the following five layers: the presentation layer, the controller layer, the application layer, the domain layer, and the infrastructure layer. The goal is to avoid the following anti-patterns typically found in many software systems: a fat application layer, an anemic domain model, and a tangled mess in general.</li>
<li>Event Sourcing: Event sourcing is an architectural pattern that stores your domain objects as a stream of events rather than just a serialization of its current state.  Each aggregate root has its own event stream which can be used to rebuild that aggregate root.  These events can be replayed at any time to perform numerous tasks like building a new report model, deriving metrics information, or for auditing purposes. http://martinfowler.com/eaaDev/EventSourcing.html</li>
<li>Security: PRO Center is built using a claims based federated security model. This allows for clean separation between authorization and authentication in an application. It also allows for an easier implementation of single sign-on across integrated applications. Users are authenticated against an Identity Provider (IDP) and is issued a token that grants access to the application. PRO Center is using an enhanced version of Thinktectures Identity Provider.
</li>
</ul>

=========
