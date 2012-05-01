/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


# Dump of table election
# ------------------------------------------------------------

DROP TABLE IF EXISTS `election`;

CREATE TABLE `election` (
  `idelection` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `latest_phase` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idelection`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table election_position
# ------------------------------------------------------------

DROP TABLE IF EXISTS `election_position`;

CREATE TABLE `election_position` (
  `idelection_position` int(11) NOT NULL AUTO_INCREMENT,
  `idelection` int(11) NOT NULL DEFAULT '1',
  `position` varchar(45) NOT NULL,
  `tally_type` varchar(9) NOT NULL DEFAULT 'Simple',
  `idum_current` int(11) DEFAULT NULL,
  `description` varchar(5000) DEFAULT NULL,
  `slots_plurality` int(11) DEFAULT NULL,
  `votes_allowed` int(11) DEFAULT NULL,
  PRIMARY KEY (`idelection_position`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table email_verification
# ------------------------------------------------------------

DROP TABLE IF EXISTS `email_verification`;

CREATE TABLE `email_verification` (
  `idemail_verification` int(11) NOT NULL AUTO_INCREMENT,
  `iduser` int(11) NOT NULL,
  `code_verified` varchar(64) DEFAULT NULL,
  `code_rejected` varchar(64) DEFAULT NULL,
  `datetime_sent` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idemail_verification`),
  UNIQUE KEY `code_verified_UNIQUE` (`code_verified`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table flag_nec
# ------------------------------------------------------------

DROP TABLE IF EXISTS `flag_nec`;

CREATE TABLE `flag_nec` (
  `idflag_nec` int(11) NOT NULL AUTO_INCREMENT,
  `idelection` int(11) NOT NULL DEFAULT '1',
  `approve` tinyint(1) NOT NULL,
  `slate` int(11) DEFAULT NULL,
  PRIMARY KEY (`idflag_nec`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table flag_voted
# ------------------------------------------------------------

DROP TABLE IF EXISTS `flag_voted`;

CREATE TABLE `flag_voted` (
  `idflag_voted` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_members` int(11) NOT NULL,
  `code_confirm` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`idflag_voted`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table nomination_accept
# ------------------------------------------------------------

DROP TABLE IF EXISTS `nomination_accept`;

CREATE TABLE `nomination_accept` (
  `idnominate_accept` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_to` int(11) NOT NULL,
  `idunion_from` int(11) NOT NULL,
  `position` varchar(45) NOT NULL,
  `accepted` tinyint(1) DEFAULT NULL,
  `from_petition` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`idnominate_accept`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table petition
# ------------------------------------------------------------

DROP TABLE IF EXISTS `petition`;

CREATE TABLE `petition` (
  `idpetition` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_members` int(11) NOT NULL,
  `positions` varchar(45) NOT NULL,
  `idum_signedby` int(11) NOT NULL,
  PRIMARY KEY (`idpetition`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table results
# ------------------------------------------------------------

DROP TABLE IF EXISTS `results`;

CREATE TABLE `results` (
  `position` varchar(45) NOT NULL,
  `id_union` int(11) NOT NULL,
  `percent` double DEFAULT NULL,
  `tally` int(11) DEFAULT NULL,
  PRIMARY KEY (`position`,`id_union`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table roles
# ------------------------------------------------------------

DROP TABLE IF EXISTS `roles`;

CREATE TABLE `roles` (
  `roles` varchar(20) NOT NULL,
  PRIMARY KEY (`roles`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;

INSERT INTO `roles` (`roles`)
VALUES
	('admin'),
	('faculty'),
	('nec');

/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

# Dump of table tally
# ------------------------------------------------------------

DROP TABLE IF EXISTS `tally`;

CREATE TABLE `tally` (
  `id_union` int(11) NOT NULL,
  `position` varchar(45) NOT NULL,
  `count` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table timeline
# ------------------------------------------------------------

DROP TABLE IF EXISTS `timeline`;

CREATE TABLE `timeline` (
  `idtimeline` int(11) NOT NULL AUTO_INCREMENT,
  `idelection` int(11) NOT NULL DEFAULT '1',
  `name_phase` varchar(45) NOT NULL,
  `datetime_end` datetime NOT NULL,
  `iscurrent` tinyint(1) NOT NULL,
  PRIMARY KEY (`idtimeline`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



# Dump of table wts
# ------------------------------------------------------------

DROP TABLE IF EXISTS `wts`;

CREATE TABLE `wts` (
  `wts_id` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_members` int(11) DEFAULT NULL,
  `position` varchar(45) DEFAULT NULL,
  `date_applied` datetime DEFAULT NULL,
  `statement` text,
  `eligible` int(11) DEFAULT NULL,
  PRIMARY KEY (`wts_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;




/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
