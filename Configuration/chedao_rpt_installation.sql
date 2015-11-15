-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: 2015-11-15 06:35:20
-- 服务器版本： 5.7.9
-- PHP Version: 5.6.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `chedao`
--

-- --------------------------------------------------------

--
-- 表的结构 `chedao_rpt_installation`
--

CREATE TABLE `chedao_rpt_installation` (
  `id` int(11) NOT NULL,
  `install_id` varchar(100) NOT NULL,
  `province` char(20) NOT NULL,
  `city` char(30) NOT NULL,
  `area` char(30) NOT NULL,
  `addr` varchar(100) NOT NULL,
  `name` char(100) NOT NULL,
  `operator` varchar(20) DEFAULT NULL,
  `other` varchar(200) DEFAULT NULL,
  `created_at` date NOT NULL,
  `updated_at` date NOT NULL,
  `upgrade_open` varchar(30) DEFAULT 'disable',
  `working_version` varchar(20) DEFAULT NULL,
  `ping_count` int(11) DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- 转存表中的数据 `chedao_rpt_installation`
--

INSERT INTO `chedao_rpt_installation` (`id`, `install_id`, `province`, `city`, `area`, `addr`, `name`, `operator`, `other`, `created_at`, `updated_at`, `upgrade_open`, `working_version`, `ping_count`) VALUES
(1, '5646c5a678032', 'asdfasd', 'adsfadsf', 'adfadsfsd', 'asdfadsfasd', 'adsfsadfds', 'asdfadsf', 'adfadfads', '2015-11-14', '2015-11-14', 'disable', NULL, 0),
(4, '56474ea81cea6', '重庆市', '重庆市', '重庆市', '23423', '23423432', '234234', 'reserved', '2015-11-14', '2015-11-14', 'disable', NULL, 0),
(5, '564751df938cc', '河北省', '石家庄市', '长安区', '345', '354354', '24352345', 'reserved', '2015-11-14', '2015-11-14', 'enable', NULL, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `chedao_rpt_installation`
--
ALTER TABLE `chedao_rpt_installation`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `install_id` (`install_id`);

--
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `chedao_rpt_installation`
--
ALTER TABLE `chedao_rpt_installation`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
