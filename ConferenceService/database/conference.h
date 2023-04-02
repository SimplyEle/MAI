#ifndef AUTHOR_H
#define AUTHOR_H

#include <string>
#include <vector>
#include "Poco/JSON/Object.h"
#include <optional>

namespace database
{
    class Conference{
        private:
            long _id;
            std::string _name_conf;
            int _organizer_id;
            int _category_id;
            std::string _description;
            std::string _date_of_conf;

        public:

            static Conference fromJSON(const std::string & str);

            long &id();
            std::string &name_conf();
            int &organizer_id();
            int &category_id();
            std::string &description();
            std::string &date_of_conf();

            static void init();
            static std::vector<Conference> read_all_confs();
            void add_conf(std::string name_conf, long organizer_id, long category_id, std::string description, std::string date_of_conf);

            Poco::JSON::Object::Ptr toJSON() const;

    };
}

#endif