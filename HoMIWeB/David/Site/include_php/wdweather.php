<?php

class WdWeather
{
    var $markup_contents = '([^<]+)';

    function getWeather($city_code, $days) {
        $xml = @file_get_contents('http://xml.weather.com/weather/local/' . $city_code . '?cc=*&unit=m&dayf=' . $days); 
        if (!$xml) {return;}
        return $this->parseDays($xml);
    }
    
    function getWeather2($city_code, $days) {
        $xml = @file_get_contents('http://xml.weather.com/weather/local/' . $city_code . '?cc=*&unit=m&dayf=' . $days);
        if (!$xml) {return;}
        return $this->parseToday($xml);
    }

    function parseDays($xml) {
        $days = array();
 	    $parts = preg_split('#<day d=[^>]+>#', $xml);
        array_shift($parts);
        foreach ($parts as $xml2) {$days[] = $this->parseDay($xml2);}
        //recup du jour/date dans le titre du day
        $x = strpos($xml,"<day d=",0);//recherche le premier jour
        while ($x>0) {
	        $temp=substr($xml,$x,(strpos($xml,">",$x)-$x));
	        preg_match('<day d="([0-9])" t="([a-zA-Z]*)" dt="([a-zA-Z0-9 ]*)">',$temp,$temp2);
	        $days[$temp2[1]]["jour"]=$this->jourstoFR($temp2[2]);
	        $days[$temp2[1]]["date"]=date("j",strtotime($temp2[3]))." ".$this->moistofr(date("m",strtotime($temp2[3])));
	        $x = strpos($xml,"<day d=",$x+1); //recherche le prochain jour
        }        
        return $days;
    }
    
    function parsetoday($xml) {
        $days = array();
        $parts = preg_split('#<cc>#', $xml);
        array_shift($parts);
        foreach ($parts as $xml) {$cc[] = $this->parseCC($xml);}
        return $cc;
    }

    function parseDay($xml) {
        $mc = $this->markup_contents;
        preg_match
        (
            '#' .
            '<hi>' . $mc . '</hi>\s*' .
            '<low>' . $mc . '</low>\s*' .
            '<sunr>' . $mc . '</sunr>\s*' .
            '<suns>' . $mc . '</suns>\s*' .
            '<part p="d">(.*)</part>\s*' .
            '<part p="n">(.*)</part>' .
            '#ms',
            $xml, $matches
        );
        array_shift($matches);         
        $matches[2] = $this->to24H($matches[2]);
        $matches[3] = $this->to24H($matches[3]);
        $matches[4] = $this->parseDayPeriod($matches[4]);
        $matches[5] = $this->parseDayPeriod($matches[5]);
        return array_combine(array('hi', 'low', 'sunr', 'suns', 'day', 'night'), $matches);
    }
   
    function parseDayPeriod($xml) {
        $mc = $this->markup_contents;
        preg_match
        (
            '#' .
            '<icon>' . $mc . '</icon>\s*' .
            '<t>' . $mc . '</t>\s*' .
            '<wind>(.*)</wind>\s*' .
            '<bt>' . $mc . '</bt>\s*' .
            '<ppcp>' . $mc . '</ppcp>\s*' .
            '<hmid>' . $mc . '</hmid>' .
            '#ms',
            $xml, $matches
        );
        array_shift($matches);
        $matches[2] = $this->parseWind($matches[2]);
        return array_combine(array('icon', 't', 'wind', 'bt', 'ppcp', 'hmid'), $matches);
    }
 
    function parseCC($xml) {
        $mc = $this->markup_contents;
        preg_match
        (
            '#' .
            '<lsup>' . $mc . '</lsup>\s*' .
            '<obst>' . $mc . '</obst>\s*' .
            '<tmp>' . $mc . '</tmp>\s*' .
            '<flik>' . $mc . '</flik>\s*' .
            '<t>' . $mc . '</t>\s*' .
            '<icon>' . $mc . '</icon>\s*' .
            '<bar>(.*)</bar>\s*' .
            '<wind>(.*)</wind>\s*' .
            '<hmid>' . $mc . '</hmid>\s*' .
            '<vis>' . $mc . '</vis>\s*' .
            '<uv>(.*)</uv>\s*' .
            '<dewp>' . $mc . '</dewp>\s*' .
            '<moon>(.*)</moon>\s*' .
            '#ms',
            $xml, $matches
        );
        array_shift($matches);             
        # parse
        $matches[6] = $this->parseBar($matches[6]);
        $matches[7] = $this->parseWind($matches[7]);
        $matches[10] = $this->parseUV($matches[10]);
        $matches[12] = $this->parseMoon($matches[12]);
        return array_combine(array('lsup', 'obst', 'tmp', 'flik', 't', 'icon', 'bar', 'wind', 'hmid', 'vis', 'uv', 'dewp', 'moon'), $matches);
    }
    
    function parseWind($xml) {
        $mc = $this->markup_contents;
        preg_match
        (
            '#' .
            '<s>' . $mc . '</s>\s*' .
            '<gust>' . $mc . '</gust>\s*' .
            '<d>' . $mc . '</d>\s*' .
            '<t>' . $mc . '</t\s*' .
            '#ms',
            $xml, $matches
        );
        array_shift($matches);
        return array_combine(array('s', 'gust', 'd', 't'), $matches);
    }
    
    function parseBar($xml) {
        $mc = $this->markup_contents;
        preg_match
        (
            '#' .
            '<r>' . $mc . '</r>\s*' .
            '<d>' . $mc . '</d>\s*' .
            '#ms',
            $xml, $matches
        );
        array_shift($matches);
        return array_combine(array('r', 'd'), $matches);
    }
    
    function parseUv($xml) {
        $mc = $this->markup_contents;
        preg_match
        (
            '#' .
            '<i>' . $mc . '</i>\s*' .
            '<t>' . $mc . '</t\s*' .
            '#ms',
            $xml, $matches
        );
        array_shift($matches);
        return array_combine(array('i', 't'), $matches);
    }
    
    function parseMoon($xml) {
        $mc = $this->markup_contents;
        preg_match
        (
            '#' .
            '<icon>' . $mc . '</icon>\s*' .
            '<t>' . $mc . '</t\s*' .
            '#ms',
            $xml, $matches
        );
        array_shift($matches);
        return array_combine(array('icon', 't'), $matches);
    }
 
    static function to24H($time) {
        preg_match('#(\d+)\:(\d+)\s+(AM|PM)?#', $time, $matches);
        if ($matches[3] == 'PM') {$matches[1] += 12;}
        return $matches[1] . ':' . $matches[2];
    }
    static function tempstoFR($temps) {
	    $temps2=strtolower(str_replace(array(" ","/"),"",$temps));
	    switch ($temps2) {
		    case "afewclouds":return "Quelques nuages";break;
			case "amcloudspmsun":return "Nuageux dans la matin�e / Soleil dans l'apr�s-midi";break;
			case "amcloudspmsunwind":return "Nuageux dans la matin�e / Soleil dans l'apr�s-midi / Vent";break;
			case "amdrizzle":return "Bruine dans la matin�e";break;
			case "amfogpmsun":return "Brouillard dans la matin�e / Soleil dans l'apr�s-midi";break;
			case "amfogpmclouds":return "Brouillard dans la matin�e / Nuageux dans l'apr�s-midi";break;
			case "amlightrain":return "L�g�re pluie dans la matin�e";break;
			case "amlightrainwind":return "L�g�re pluie dans la matin�e / Vent";break;
			case "amlightsnow":return "L�g�res chutes de neige dans la matin�e";break;
			case "amlightsnowwind":return "L�g�res chutes de neige dans la matin�e / Vent";break;
			case "amlightwintrymix":return "L�g�res pr�cipitations hivernales dans la matin�e";break;
			case "amrain":return "Pluie dans la matin�e";break;
			case "amrainice":return "Pluie dans la matin�e / Glace";break;
			case "amrainsnow":return "Pluie dans la matin�e / Neige";break;
			case "amrainsnowwind":return "Pluie dans la matin�e / Neige / Vent";break;
			case "amrainsnowshowers":return "Pluie / Chutes de neige dans la matin�e";break;
			case "amrainwind":return "Pluie dans la matin�e / Vent";break;
			case "amshowers":return "Averses dans la matin�e";break;
			case "amshowerswind":return "Averses dans la matin�e / Vent";break;
			case "amsnow":return "Neige dans la matin�e";break;
			case "amsnowshowers":return "Chutes de neige dans la matin�e";break;
			case "amsnowshowerswind":return "Chutes de neige dans la matin�e / Vent";break;
			case "amt-storms":return "Orages dans la matin�e";break;
			case "amwintrymix":return "Pr�cipitations hivernales dans la matin�e";break;
			case "blowingsandandwindy":return "Rafales de sable et Vent";break;
			case "blowingsnow":return "Rafales de neige";break;
			case "clear":return "Ciel d�gag�";break;
			case "cloudsearlyclearinglate":return "Nuageux d'abord / �claircie ensuite";break;
			case "cloudy":return "Nuageux";break;
			case "cloudywind":return "Nuageux / Vent";break;
			case "cloudyandwindy":return "Nuageux et Venteux";break;
			case "driftingsnow":return "Rafales de neige";break;
			case "drizzle":return "Bruine";break;
			case "drizzlefog":return "Bruine / Brouillard";break;
			case "fair":return "Beau";break;
			case "fairandwindy":return "Beau et Venteux";break;
			case "fewshowers":return "Quelques averses";break;
			case "fewshowerswind":return "Quelques averses / Vent";break;
			case "fewsnowshowers":return "Quelques chutes de neige";break;
			case "fewsnowshowerswind":return "Quelques chutes de neige / Vent";break;
			case "flurries":return "Chutes de neige fondante";break;
			case "flurrieswind":return "Chutes de neige fondante / Vent";break;
			case "fog":return "Brouillard";break;
			case "fogclouds":return "Brouillard / Ciel se couvrant";break;
			case "foggy":return "Brouillard";break;
			case "foglate":return "Brouillard";break;
			case "freezingrain":return "Pluie vergla�ante";break;
			case "haze":return "L�g�re brume";break;
			case "heavydrizzle":return "Forte bruine";break;
			case "heavyrain":return "Forte pluie";break;
			case "heavyrainwind":return "Forte pluie / Vent";break;
			case "heavyrainshower":return "Fortes averses";break;
			case "heavyrainshowerandwindy":return "Fortes averses et Vent";break;
			case "heavysnow":return "Fortes chutes de neige";break;
			case "heavyt-storm":return "Orage violent";break;
			case "heavyt-stormandwindy":return "Orage violent et Vent";break;
			case "heavyt-storms":return "Orages violents";break;
			case "heavyt-stormswind":return "Orages violents / Vent";break;
			case "hvyrainfreezingrain":return "Forte pluie / Pluie vergla�ante";break;
			case "icetorain":return "Givre puis Pluie";break;
			case "isot-stormswind":return "Orages isol�s / Vent";break;
			case "isolatedt-storms":return "Orages isol�s";break;
			case "lightdrizzle":return "L�g�re bruine";break;
			case "lightfreezingdrizzle":return "L�g�re bruine vergla�ante";break;
			case "lightfreezingrain":return "L�g�re pluie vergla�ante";break;
			case "lightrain":return "L�g�re pluie";break;
			case "lightrainearly":return "L�g�re pluie";break;
			case "lightrainfog":return "L�g�re pluie / Brouillard";break;
			case "lightrainfreezingrain":return "L�g�re pluie / Pluie vergla�ante";break;
			case "lightrainlate":return "L�g�re pluie tardive";break;
			case "lightrainwind":return "L�g�re pluie / Vent";break;
			case "lightrainandfog":return "L�g�re pluie et Brouillard";break;
			case "lightrainandfreezingrain":return "L�g�re pluie et Pluie vergla�ante";break;
			case "lightrainandwindy":return "L�g�re pluie et Vent";break;
			case "lightrainshower":return "L�g�res averses";break;
			case "lightrainshowerandwindy":return "L�g�res averses et Vent";break;
			case "lightrainwiththunder":return "L�g�re pluie avec tonnerre";break;
			case "lightsnow":return "L�g�res chutes de neige";break;
			case "lightsnowwind":return "L�g�re neige / Vent";break;
			case "lightsnowandfog":return "L�g�re neige et Brouillard";break;
			case "lightsnowfog":return "L�g�re neige et Brouillard";break;
			case "lightsnowearly":return "L�g�res chutes de neige";break;
			case "lightsnowgrains":return "L�gers granules de neige";break;
			case "lightsnowgrainsandfog":return "L�gers granules de neige et Brouillard";break;
			case "lightsnowlate":return "L�g�res chutes de neige";break;
			case "lightsnowshower":return "L�g�res chutes de neige";break;
			case "lightsnowshowerwind":return "L�g�res chutes de neige / Vent";break;
			case "mist":return "Brume";break;
			case "mostlyclear":return "Ciel plut�t d�gag�";break;
			case "mostlyclearwind":return "Ciel plut�t d�gag� / Vent";break;
			case "mostlycloudy":return "Plut�t nuageux";break;
			case "mostlycloudywind":return "Plut�t nuageux / Vent";break;
			case "mostlycloudyandwindy":return "Plut�t nuageux et Venteux";break;
			case "mostlysunny":return "Plut�t ensoleill�";break;
			case "mostlysunnywind":return "Plut�t ensoleill� / Vent";break;
			case "partlycloudy":return "Passages nuageux";break;
			case "partlycloudywind":return "Passages nuageux / Vent";break;
			case "partlycloudyandwindy":return "Passages nuageux et Vent";break;
			case "patchesoffog":return "Bancs de brouillard";break;
			case "pmdrizzle":return "Bruine dans l'apr�s-midi";break;
			case "pmfog":return "Brouillard dans l'apr�s-midi";break;
			case "pmlightrain":return "L�g�re pluie dans l'apr�s-midi";break;
			case "pmlightrainice":return "L�g�re pluie dans l'apr�s-midi / Verglas";break;
			case "pmlightrainwind":return "L�g�re pluie dans l'apr�s-midi / Vent";break;
			case "pmlightsnow":return "L�g�res chutes de neige dans l'apr�s-midi";break;
			case "pmlightsnowwind":return "L�g�res chutes de neige dans l'apr�s-midi / Vent";break;
			case "pmrain":return "Pluie dans l'apr�s-midi";break;
			case "pmrainsnow":return "Pluie / Neige dans l'apr�s-midi";break;
			case "pmrainsnowwind":return "Pluie / Neige / Vent dans l'apr�s-midi";break;
			case "pmrainsnowshowers":return "Pluie / Chutes de neige dans l'apr�s-midi";break;
			case "pmshowers":return "Averses dans l'apr�s-midi";break;
			case "pmshowerswind":return "Averses / Vent dans l'apr�s-midi";break;
			case "pmsnow":return "Chutes de neige dans l'apr�s-midi";break;
			case "pmsnowwind":return "Chutes de neige dans l'apr�s-midi / Vent";break;
			case "pmsnowshowers":return "Chutes de neige dans l'apr�s-midi";break;
			case "pmsnowshowerswind":return "Chutes de neige dans l'apr�s-midi / Vent";break;
			case "pmt-showers":return "Averses orageuses dans l'apr�s-midi";break;
			case "pmt-storms":return "Orages dans l'apr�s-midi";break;
			case "pmwintrymix":return "Pr�cipitations hivernales dans l'apr�s-midi";break;
			case "rain":return "Pluie";break;
			case "rainfreezingrain":return "Pluie / Pluie verglaçante";break;
			case "rainsleet":return "Pluie / Granules de glace";break;
			case "rainsnow":return "Pluie / Neige";break;
			case "rainsnowlate":return "Pluie / Neige tardive";break;
			case "rainsnowwind":return "Pluie / Neige / Vent";break;
			case "rainsnowshowers":return "Pluie / Chutes de neige";break;
			case "rainsnowshowerswind":return "Pluie / Chutes de neige / Vent";break;
			case "rainsnowshowerslate":return "Pluie / Chutes de neige";break;
			case "rainthunder":return "Pluie / Tonnerre";break;
			case "rainthunderwind":return "Pluie / Tonnerre / Vent";break;
			case "rainwind":return "Pluie / Vent";break;
			case "rainandsleet":return "Pluie et Granules de glace";break;
			case "rainandsnow":return "Pluie et Neige";break;
			case "rainshower":return "Averses";break;
			case "rainshowerandwindy":return "Averses et Vent";break;
			case "raintosnow":return "Pluie devenant neige";break;
			case "raintosnowwind":return "Pluie devenant neige / Vent";break;
			case "scatteredflurries":return "Chutes de neige fondante �parses";break;
			case "scatteredflurrieswind":return "Chutes de neige fondante �parses / Vent";break;
			case "scatteredshowers":return "Averses �parses";break;
			case "scatteredshowerswind":return "Averses �parses / Vent";break;
			case "scatteredsnowshowers":return "Alternance de chutes de neige";break;
			case "scatteredsnowshowerswind":return "Chutes de neige �parses / Vent";break;
			case "scatteredstrongstorms":return "Orages violents �pars";break;
			case "scatteredt-storms":return "Orages �pars";break;
			case "scatteredt-stormswind":return "Orages �pars / Vent";break;
			case "shallowfog":return "Brouillard";break;
			case "showers":return "Averses";break;
			case "showerswind":return "Averses / Vent";break;
			case "showerswindlate":return "Averses / Vent";break;
			case "showersearly":return "Averses";break;
			case "showersinthevicinity":return "Averses dans le voisinage";break;
			case "showerslate":return "Averses";break;
			case "sleet":return "Granules de glace ";break;
			case "smoke":return "Fum�e";break;
			case "snow":return "Neige";break;
			case "snowwind":return "Neige / Vent";break;
			case "snowandfog":return "Neige et Brouillard";break;
			case "snowandicetorain":return "Neige et Glace puis Pluie";break;
			case "snowgrains":return "Granules de neige";break;
			case "snowshower":return "Chutes de neige";break;
			case "snowshowerwind":return "Chutes de neige / Vent";break;
			case "snowshowerwindearly":return "Chutes de neige / Vent";break;
			case "snowshowersearly":return "Chutes de neige";break;
			case "snowshowerslate":return "Chutes de neige tardive";break;
			case "snowtoice":return "Neige puis Verglas";break;
			case "snowtoicewind":return "Neige se transformant en glace / Vent";break;
			case "snowtorain":return "Neige puis Pluie";break;
			case "snowtorainwind":return "Neige puis Pluie / Vent";break;
			case "snowtowintrymix":return "Neige puis Pr�cipitations hivernales";break;
			case "sprinkles":return "Averses";break;
			case "strongstorms":return "Orages violents";break;
			case "strongstormswind":return "Orages violents / Vent";break;
			case "sunny":return "Ensoleill�";break;
			case "sunnywind":return "Ensoleill� / Vent";break;
			case "sunnyandwindy":return "Ensoleill� et Venteux";break;
			case "t-showers":return "Averses orageuses";break;
			case "t-showerswind":return "Averses orageuses / Vent";break;
			case "t-storm":return "Orage";break;
			case "t-storms":return "Orages";break;
			case "t-stormswind":return "Orages / Vent";break;
			case "t-stormsearly":return "Orages";break;
			case "t-stormslate":return "Orages";break;
			case "thunder":return "Tonnerre";break;
			case "thunderandwintrymix":return "Tonnerre et Pr�cipitations hivernales";break;
			case "thunderinthevicinity":return "Tonnerre dans le voisinage";break;
			case "unknownprecip":return "Pr�cipitations";break;
			case "widespreaddust":return "Brume s�che";break;
			case "wintrymix":return "Pr�cipitations hivernales";break;
			case "wintrymixwind":return "Pr�cipitations hivernales / Vent";break;
			case "wintrymixtosnow":return "Pr�cipitations hivernales puis Neige";break;
		    default : return $temps;break;
	    }
    }
    function jourstoFR($texte) {
        $jourfr["monday"]="Lundi";
	    $jourfr["tuesday"]="Mardi";
	    $jourfr["wednesday"]="Mercredi";
	    $jourfr["thursday"]="Jeudi";
	    $jourfr["friday"]="Vendredi";
	    $jourfr["saturday"]="Samedi";
	    $jourfr["sunday"]="Dimanche";
	    if(array_key_exists(strtolower($texte),$jourfr)) {return $jourfr[strtolower($texte)];} else {return $texte;}
    }
    
    function moistoFR($nb) {
	    $nb=(int)$nb;
        $moisfr[1]="Janvier";
	    $moisfr[2]="F�vrier";
	    $moisfr[3]="Mars";
	    $moisfr[4]="Avril";
	    $moisfr[5]="Mai";
	    $moisfr[6]="Juin";
	    $moisfr[7]="Juillet";
	    $moisfr[8]="Aout";
	    $moisfr[9]="Septembre";
	    $moisfr[10]="Octobre";
	    $moisfr[11]="Novembre";
	    $moisfr[12]="Decembre";
	    return $moisfr[$nb];
    }
}

?>