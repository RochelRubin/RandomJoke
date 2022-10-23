import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

import { useAuthContext } from '../AuthContext';
const HomePage = () => {

    const [joke, setJoke] = useState({
        setup:'',
        punchline:''
    });
    const [likes, setLikes] = useState(0);
    const [dislikes, setDislikes] = useState(0);
    const [alreadyLiked, setLiked] = useState(false);
    const [alreadyDisliked, setDisliked] = useState(false);
    let x = 0;

    const { user } = useAuthContext();

    useEffect(() => {
        getJoke();

        setInterval(async () => {
            getLikes(x);
        }, 500);
    }, [])

    const getJoke = async () => {
        const { data } = await axios.get('/api/joke/getJoke');
        setJoke(data);
        getLikes(data.id);
        x = data.id;
        if (user) {
            const { data } = await axios.get(`/api/joke/IfLiked?jokeId=${x}`);
            setDisliked(data.disliked);
            setLiked(data.liked);
        }
    }

    const getLikes = async id => {
        const { data } = await axios.get(`/api/joke/getLikes?id=${id}`);
        setLikes(data.filter(l => l.liked === true).length);
        setDislikes(data.filter(l => l.liked === false).length);
    }

    const onLikeClick = async () => {
        await axios.post('/api/joke/addLike', joke);
        setLiked(true);
        setDisliked(false);

        getLikes(joke.id);
    }

    const onDislikeClick = async () => {
        await axios.post('/api/joke/adddislike', joke);
        setDisliked(true);
        setLiked(false);

        getLikes(joke.id);
    }

    return (<div className="col-md-6 offset-md-3 card card-body bg-light">
        <div>
            <h4>{joke.setup}</h4>
            <h4>{joke.punchline}</h4>
            {!user &&
                <Link to="/login">Login to your account to like/dislike this joke</Link>
            }
            <div>
                {user &&
                    <div>
                        <button className="btn btn-primary" disabled={alreadyLiked} onClick={onLikeClick}>Like</button>
                        <button className="btn btn-danger" disabled={alreadyDisliked} onClick={onDislikeClick}>Dislike</button>
                    </div>}
                <br />
                <h4>Likes: {likes}</h4>
                <h4>Dislikes: {dislikes}</h4>
                <h4>
                    <button onClick={() => window.location.reload(false)} className="btn btn-link">Refresh</button>
                </h4>
            </div>
        </div>
    </div >)
}
export default HomePage;
