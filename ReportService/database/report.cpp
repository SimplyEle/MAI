#include "report.h"
#include "database.h"
#include "../config/config.h"

#include <Poco/Data/MySQL/Connector.h>
#include <Poco/Data/MySQL/MySQLException.h>
#include <Poco/Data/SessionFactory.h>
#include <Poco/Data/RecordSet.h>
#include <Poco/JSON/Parser.h>
#include <Poco/Dynamic/Var.h>

#include <sstream>
#include <exception>

using namespace Poco::Data::Keywords;
using Poco::Data::Session;
using Poco::Data::Statement;

namespace database
{

    void Report::init()
    {
        try
        {

            Poco::Data::Session session = database::Database::get().create_session();
            Statement create_stmt(session);
            create_stmt << "CREATE TABLE `Report`(`id int NOT NULL AUTO_INCREMENT,"
                        << "`name_report`   text NOT NULL ,"
                        << "`author_id`     int NOT NULL ,"
                        << "`annotation`    text NOT NULL ,"
                        << "`text_report`   text NOT NULL ,"
                        << "`date_creation` date NOT NULL ,"

                        << "PRIMARY KEY (`id`),"
                        << "KEY `FK_2` (`author_id`),"
                        << "CONSTRAINT `FK_2` FOREIGN KEY `FK_2` (`author_id`) REFERENCES `User` (`id`));",
                now;
        }

        catch (Poco::Data::MySQL::ConnectionException &e)
        {
            std::cout << "connection:" << e.what() << std::endl;
            throw;
        }
        catch (Poco::Data::MySQL::StatementException &e)
        {

            std::cout << "statement:" << e.what() << std::endl;
            throw;
        }
    }

    Poco::JSON::Object::Ptr Report::toJSON() const
    {
        Poco::JSON::Object::Ptr root = new Poco::JSON::Object();

        root->set("id", _id);
        root->set("name_report", _name_report);
        root->set("author_id", _author_id);
        root->set("annotation", _annotation);
        root->set("text_report", _text_report);
        root->set("date_creation", _date_creation);

        return root;
    }

    Report Report::fromJSON(const std::string &str)
    {
        Report report;
        Poco::JSON::Parser parser;
        Poco::Dynamic::Var result = parser.parse(str);
        Poco::JSON::Object::Ptr object = result.extract<Poco::JSON::Object::Ptr>();

        report.id() = object->getValue<long>("id");
        report.first_name() = object->getValue<std::string>("name_report");
        report.last_name() = object->getValue<long>("author_id");
        report.email() = object->getValue<std::string>("annotation");
        report.title() = object->getValue<std::string>("text_report");
        report.login() = object->getValue<std::string>("date_creation");

        return report;
    }

    std::optional<Report> Report::update_by_id(long id, std::string name_report, long author_id, std::string annotation, std::string text_report, std::string date_creation)
    {
        try
        {
            Poco::Data::Session session = database::Database::get().create_session();
            Poco::Data::Statement update(session);
            Report a;
            update << "UPDATE id, name_report, author_id, annotation, text_report, date_creation FROM Report where id=?",
                use(a.id),
                use(a.name_report),
                use(a.author_id),
                use(a.annotation),
                use(a.text_report),
                use(a.date_creation)

            update.execute();

            std::cout << "updated:" << _id << std::endl;
        }

        catch (Poco::Data::MySQL::ConnectionException &e)
        {
            std::cout << "connection:" << e.what() << std::endl;
        }
        catch (Poco::Data::MySQL::StatementException &e)
        {
            std::cout << "statement:" << e.what() << std::endl;   
        }
        return {};
    }

    std::optional<Report> Report::search_report(long id)
    {
        try
        {
            Poco::Data::Session session = database::Database::get().create_session();
            Poco::Data::Statement select(session);
            Report a;
            select << "SELECT id, name_report, author_id, annotation, text_report, date_creation FROM Report where id=?",
                into(a._id),
                into(a._name_report),
                into(a._author_id),
                into(a._annotation),
                into(a._text_report),
                into(a._date_creation),
                use(id),
                range(0, 1); //  iterate over result set one row at a time

            select.execute();
            Poco::Data::RecordSet rs(select);
            if (rs.moveFirst()) return a;
        }

        catch (Poco::Data::MySQL::ConnectionException &e)
        {
            std::cout << "connection:" << e.what() << std::endl;
        }
        catch (Poco::Data::MySQL::StatementException &e)
        {
            std::cout << "statement:" << e.what() << std::endl;   
        }
        return {};
    }

    void Report::add_report(std::string name_report, long author_id, std::string annotation, std::string text_report, std::string date_creation)
    {

        try
        {
            Poco::Data::Session session = database::Database::get().create_session();
            Poco::Data::Statement insert(session);

            insert << "INSERT INTO Report (name_report, author_id, annotation, text_report, date_creation) VALUES(?, ?, ?, ?, ?)",
                use(name_report),
                use(author_id),
                use(annotation),
                use(text_report),
                use(date_creation);

            insert.execute();

            std::cout << "inserted:" << _id << std::endl;
        }
        catch (Poco::Data::MySQL::ConnectionException &e)
        {
            std::cout << "connection:" << e.what() << std::endl;
            throw;
        }
        catch (Poco::Data::MySQL::StatementException &e)
        {
            std::cout << "statement:" << e.what() << std::endl;
            throw;
        }
    }

    long &Report::id()
    {
        return _id;
    }

    std::string &Report::name_report()
    {
        return _name_report;
    }

    long &Report::author_id()
    {
        return _author_id;
    }
    
    std::string &Report::annotation()
    {
        return _annotation;
    }

    std::string &Report::text_report()
    {
        return _text_report;
    }

    std::string &Report::date_creation()
    {
        return _date_creation;
    }
}