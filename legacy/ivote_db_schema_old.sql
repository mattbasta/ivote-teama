-- Created by Team A for CSC 354, Fall 2011
-- 
-- Developed by Adam Blank, Kenneth Rohlfing, and Ralph Sharp
--
-- Host: localhost
-- Generation Time: Dec 08, 2011 at 08:28 PM
-- Server version: 5.1.52
-- PHP Version: 5.2.14

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";

--
-- Database: `ivote`
--

-- --------------------------------------------------------

--
-- Table structure for table `election`
--

CREATE SCHEMA IF NOT EXISTS ivote

USE ivote;

DROP TABLE IF EXISTS `election`;
CREATE TABLE IF NOT EXISTS `election` (
  `idelection` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `latest_phase` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idelection`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;


-- --------------------------------------------------------

--
-- Table structure for table `election_position`
--

DROP TABLE IF EXISTS `election_position`;
CREATE TABLE IF NOT EXISTS `election_position` (
  `idelection_position` int(11) NOT NULL AUTO_INCREMENT,
  `idelection` int(11) NOT NULL DEFAULT '1',
  `position` varchar(45) NOT NULL,
  `tally_type` varchar(9) NOT NULL DEFAULT 'classic',
  `idum_current` int(11) DEFAULT NULL,
  `description` varchar(5000) DEFAULT NULL,
  `slots_plurality` int(11) DEFAULT NULL,
  `votes_allowed` int(11) DEFAULT NULL,
  PRIMARY KEY (`idelection_position`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;


-- --------------------------------------------------------

--
-- Table structure for table `email_verification`
--

DROP TABLE IF EXISTS `email_verification`;
CREATE TABLE IF NOT EXISTS `email_verification` (
  `idemail_verification` int(11) NOT NULL AUTO_INCREMENT,
  `iduser` int(11) NOT NULL,
  `code_verified` varchar(64) DEFAULT NULL,
  `code_rejected` varchar(64) DEFAULT NULL,
  `datetime_sent` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idemail_verification`),
  UNIQUE KEY `code_verified_UNIQUE` (`code_verified`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=172 ;

-- --------------------------------------------------------

--
-- Table structure for table `flag_nec`
--

DROP TABLE IF EXISTS `flag_nec`;
CREATE TABLE IF NOT EXISTS `flag_nec` (
  `idflag_nec` int(11) NOT NULL AUTO_INCREMENT,
  `idelection` int(11) NOT NULL DEFAULT '1',
  `approve` tinyint(1) NOT NULL,
  `slate` int(11) DEFAULT NULL,
  PRIMARY KEY (`idflag_nec`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;


-- --------------------------------------------------------

--
-- Table structure for table `flag_voted`
--

DROP TABLE IF EXISTS `flag_voted`;
CREATE TABLE IF NOT EXISTS `flag_voted` (
  `idflag_voted` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_members` int(11) NOT NULL,
  `code_confirm` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`idflag_voted`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=5 ;


-- --------------------------------------------------------

--
-- Table structure for table `nomination_accept`
--

DROP TABLE IF EXISTS `nomination_accept`;
CREATE TABLE IF NOT EXISTS `nomination_accept` (
  `idnominate_accept` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_to` int(11) NOT NULL,
  `idunion_from` int(11) NOT NULL,
  `position` varchar(45) NOT NULL,
  `accepted` tinyint(1) DEFAULT NULL,
  `from_petition` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`idnominate_accept`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=8 ;


-- --------------------------------------------------------

--
-- Table structure for table `petition`
--

DROP TABLE IF EXISTS `petition`;
CREATE TABLE IF NOT EXISTS `petition` (
  `idpetition` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_members` int(11) NOT NULL,
  `positions` varchar(20) NOT NULL,
  `idum_signedby` int(11) NOT NULL,
  PRIMARY KEY (`idpetition`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;



-- --------------------------------------------------------

--
-- Table structure for table `results`
--

DROP TABLE IF EXISTS `results`;
CREATE TABLE IF NOT EXISTS `results` (
  `position` varchar(20) NOT NULL,
  `id_union` int(11) NOT NULL,
  `percent` double DEFAULT NULL,
  `tally` int(11) DEFAULT NULL,
  PRIMARY KEY (`position`,`id_union`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
CREATE TABLE IF NOT EXISTS `roles` (
  `roles` varchar(20) NOT NULL,
  PRIMARY KEY (`roles`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- NEEDED data for table `roles`
--

INSERT INTO `roles` (`roles`) VALUES
('admin'),
('faculty'),
('nec');

-- --------------------------------------------------------

--
-- Table structure for table `roles_users`
--

DROP TABLE IF EXISTS `roles_users`;
CREATE TABLE IF NOT EXISTS `roles_users` (
  `idroles_users` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `role` varchar(22) NOT NULL,
  PRIMARY KEY (`idroles_users`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=148 ;

--
-- NEEDED data for table `roles_users`
--

INSERT INTO `roles_users` (`idroles_users`, `username`, `role`) VALUES
(1, 'admin', 'admin');

-- --------------------------------------------------------

--
-- Table structure for table `tally`
--

DROP TABLE IF EXISTS `tally`;
CREATE TABLE IF NOT EXISTS `tally` (
  `id_union` int(11) NOT NULL,
  `position` varchar(20) NOT NULL,
  `count` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_union`,`position`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


-- --------------------------------------------------------

--
-- Table structure for table `timeline`
--

DROP TABLE IF EXISTS `timeline`;
CREATE TABLE IF NOT EXISTS `timeline` (
  `idtimeline` int(11) NOT NULL AUTO_INCREMENT,
  `idelection` int(11) NOT NULL DEFAULT '1',
  `name_phase` varchar(45) NOT NULL,
  `datetime_end` datetime NOT NULL,
  `iscurrent` tinyint(1) NOT NULL,
  PRIMARY KEY (`idtimeline`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=10 ;

-- --------------------------------------------------------

--
-- Table structure for table `union_members`
--

DROP TABLE IF EXISTS `union_members`;
CREATE TABLE IF NOT EXISTS `union_members` (
  `idunion_members` int(11) NOT NULL AUTO_INCREMENT,
  `last_name` varchar(45) DEFAULT NULL,
  `first_name` varchar(45) DEFAULT NULL,
  `email` varchar(45) NOT NULL,
  `phone` varchar(45) DEFAULT NULL,
  `department` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idunion_members`),
  FULLTEXT KEY `fulltext` (`last_name`,`first_name`,`department`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=215 ;

--
-- NEEDED data for table `union_members`
--

INSERT INTO `union_members` (`idunion_members`, `last_name`, `first_name`, `email`, `phone`, `department`) VALUES
(1, 'Admin', 'Account', 'changetorealemail@doitstat.org', '', '');


-- --------------------------------------------------------

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `iduser` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_members` int(11) NOT NULL,
  `username` varchar(45) DEFAULT NULL,
  `password` varchar(64) DEFAULT NULL,
  `password_hint` varchar(200) DEFAULT NULL,
  `datetime_lastLogin` datetime DEFAULT NULL,
  PRIMARY KEY (`iduser`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=67 ;

--
-- NEEDED data for table `user`
--

INSERT INTO `user` (`iduser`, `idunion_members`, `username`, `password`, `password_hint`, `datetime_lastLogin`) VALUES
(1, 1, 'admin', 'C6rVIVPg5o5ktrxyEGLGR9nz20+1U4OuXR4bxe/kBdE=', NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `wts`
--

DROP TABLE IF EXISTS `wts`;
CREATE TABLE IF NOT EXISTS `wts` (
  `wts_id` int(11) NOT NULL AUTO_INCREMENT,
  `idunion_members` int(11) DEFAULT NULL,
  `position` varchar(20) DEFAULT NULL,
  `date_applied` datetime DEFAULT NULL,
  `statement` varchar(1024) DEFAULT NULL,
  `eligible` int(11) DEFAULT NULL,
  PRIMARY KEY (`wts_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=7 ;

