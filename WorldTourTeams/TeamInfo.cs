using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTourTeams {
    public class TeamInfo {

        public String teamName { get; set; }
        public String teamShirt { get; set; }

        public TeamInfo(String teamName, String teamShirt, String teamAbbreviation) {
            this.teamName = teamName;
            this.teamShirt = teamShirt;
            this.teamAbbreviation = teamAbbreviation;
        }

        public string teamAbbreviation { get; set; }
    }
}
