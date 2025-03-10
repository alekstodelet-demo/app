import PropTypes, { bool } from 'prop-types';

// material-ui
import { useTheme } from '@mui/material/styles';
import Avatar from '@mui/material/Avatar';
import Box from '@mui/material/Box';
import ButtonBase from '@mui/material/ButtonBase';

// project imports
import LogoSection from '../LogoSection';
// import SearchSection from './SearchSection';
import NotificationSection from './NotificationSection';
import ProfileSection from './ProfileSection';
import LanguageSection from './LanguageSection';

// assets
import { IconMenu2 } from '@tabler/icons-react';
import { observer } from 'mobx-react';
import store from 'layouts/MainLayout/store'
import { Row } from 'primereact/row';
import styled from 'styled-components';
import { Link, useLocation } from "react-router-dom";
import CustomButton from 'components/Button';
import mainStore from "../../../MainStore";
import { Feedback } from "@mui/icons-material";

// ==============================|| MAIN NAVBAR / HEADER ||============================== //

const Header = observer(({ handleLeftDrawerToggle }) => {
  const location = useLocation()
  const theme = useTheme();

  let isPortal = localStorage.getItem("portal") != null;

  return (
    <>
      {/* logo & toggler button */}
      <Box
        sx={{
          // width: 228,
          display: 'flex',
          [theme.breakpoints.down('md')]: {
            width: 'auto'
          }
        }}
      >
        <Box component="span" sx={{ display: { xs: 'none', md: 'block' }, flexGrow: 1 }}>
          <LogoSection />
        </Box>
        <ButtonBase sx={{ borderRadius: '8px', overflow: 'hidden', minWidth: 100 }}>
          <Avatar
            variant="rounded"
            sx={{
              // ...theme.typography.commonAvatar,
              // ...theme.typography.mediumAvatar,
              transition: 'all .2s ease-in-out',
              background: theme.palette.secondary.light,
              color: theme.palette.secondary.dark,
              '&:hover': {
                background: theme.palette.secondary.dark,
                color: theme.palette.secondary.light
              }
            }}
            onClick={handleLeftDrawerToggle}
            color="inherit"
          >
            <IconMenu2 stroke={1.5} size="1.3rem" />
          </Avatar>
        </ButtonBase>

        <Box display={"flex"} sx={{ ml: 5 }}>
          {!mainStore.myRoles.includes("admin") && (
            mainStore.menuHeader.map((menu) => (
              <ItemMenu $active={location.pathname.includes(menu.url)}>
                <Link id={`MenuHeader${menu.id}`} to={menu.url}>{menu.title}</Link>
              </ItemMenu>
            ))
          )}
          {/* {(MainStore.isEmployee || mainStore.isHeadStructure) && <ItemMenu $active={location.pathname.includes('/user/my_tasks')}>
            <Link id={"MenuItemMyTasks"} to='/user/my_tasks'>Мои задачи</Link>
          </ItemMenu>}
          {(MainStore.isEmployee || mainStore.isHeadStructure) && <ItemMenu $active={location.pathname.includes('/user/structure_tasks')}>
            <Link id={"MenuItemTasks"} to='/user/structure_tasks'>Задачи</Link>
          </ItemMenu>}
          <ItemMenu $active={location.pathname.includes('/user/ArchiveLog')}>
            <Link id={"MenuItemArchive"} to={'/user/ArchiveLog'}>Архив</Link>
          </ItemMenu> */}

        </Box>


      </Box>
      {isPortal &&
        <Box ml={1}><div style={{ fontSize: '1.5em' }}>Портал Согласования</div></Box>}
      {/* header search */}
      {/* <SearchSection /> */}
      <Box sx={{ flexGrow: 1 }} />
      <Box sx={{ flexGrow: 1 }} />

      <LanguageSection style={{ marginLeft: '0px' }} />
      <NotificationSection lengthNotify={store.notifications?.length} />
      <ProfileSection />
    </>
  );
});


const ItemMenu = styled.li`
  list-style-type: none;

  a {
    cursor: pointer;
    display: flex;
    align-items: center;
    min-width: ${(props) => props.$minWidth};;

    font-family: Roboto, sans-serif;
    font-size: 14px;
    font-weight: 500;
    line-height: 20px;
    text-align: left;
    pointer-events: ${(props) => props.$disabled && `none`};
    color: ${(props) => props.$disabled ? "var(--colorNeutralForeground5)" : props.$active ? "var(--colorBrandForeground1)" : "var(--colorPaletteVioletBackground1)"};
    width: fit-content;
    border-bottom: ${(props) => props.$active && `4px solid var(--colorBrandForeground1)`};
    height: 100%;
    padding: ${(props) => (props.$active ? "4px" : "0px")} 16px 0;

    &:hover {
      text-decoration: none !important;
    }
  }
`;

export default Header;
