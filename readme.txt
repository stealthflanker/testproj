euro-fatca - simple WPF app for standart fatca report to USA IRS via IDES
1) main idea - to view app with UI and MVVM.

ReferenceService - microservice based on windows service and webapi.
Tech stack:
1) topshelf - for release windows service
2) Autofac, NUnit - for unit tests purposes (injection and autogenerate test objects)
3) NLog- for logging util
4) npgsql, EntityFramework - for PostgreSQL with help of ORM
5) integration with SOAP services