#ifndef AUTHOR_H
#define AUTHOR_H

#include <string>
#include <vector>
#include "Poco/JSON/Object.h"
#include <optional>

namespace database
{
    class Category{
        private:
            long _id;
            std::string _name_of_category;
        public:

            static Category fromJSON(const std::string & str);

            long &conf_id();
            long &report_id();

            static void init();
            void add_report_to_conf(long report_id, long conf_id);
            static std::vector<Conference> read_all_reports_from_conf(long conf_id);

            Poco::JSON::Object::Ptr toJSON() const;

    };
}

#endif