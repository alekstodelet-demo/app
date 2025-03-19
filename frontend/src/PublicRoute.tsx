import { Outlet, Navigate, useLocation } from "react-router-dom";
import { useEffect, useState } from "react";
import MainStore from "./MainStore";
import { Backdrop, CircularProgress } from "@mui/material";
import { observer } from "mobx-react";
import ConfirmDialog from "components/ConfirmDialog";
import AuthStore from "features/Auth/store";

const PublicRoute = observer(() => {
    const location = useLocation();
    // Убираем состояния и useEffect
    // const [isChecking, setIsChecking] = useState(true);
    // const [isAuthenticated, setIsAuthenticated] = useState(false);

    // useEffect(() => {
    //     const checkAuthentication = async () => {
    //         setIsChecking(true);
    //         const authenticated = await AuthStore.checkAuth();
    //         setIsAuthenticated(authenticated);
    //         setIsChecking(false);
    //     };

    //     checkAuthentication();
    // }, []);

    // if (isChecking) {
    //     return (
    //         <Backdrop sx={{ color: "#fff", zIndex: 1000000 }} open={true}>
    //             <CircularProgress color="inherit" />
    //         </Backdrop>
    //     );
    // }

    // if (isAuthenticated) {
    //     return <Navigate to="/user" state={{ from: location }} replace />;
    // }

    return (
        <>
            <ConfirmDialog />
            <Backdrop id="Preloader_1" sx={{ color: "#fff", zIndex: 1000000 }} open={MainStore.loader}>
                <CircularProgress color="inherit" />
            </Backdrop>
            <Outlet />
        </>
    );
});

export default PublicRoute;