import React, {createContext, useContext, useEffect,  useState} from "react";

import axios from "axios";

const AuthContext = createContext();

const AuthContextComponent = ({children}) => {

    const [user, setUser] = useState(null);

   useEffect(() => {
        const getUser = async () => {
            const { data } = await axios.get('/api/account/getcurrentuser');
            setUser(data);
        }

        getUser();
    }, []);


    return <AuthContext.Provider value={{user, setUser}}>
            {children}
        </AuthContext.Provider>

}

const useAuthContext = () => useContext(AuthContext);


export { AuthContextComponent, useAuthContext};